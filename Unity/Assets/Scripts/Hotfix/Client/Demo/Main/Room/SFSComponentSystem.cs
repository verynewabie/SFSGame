using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSComponent))]
    [FriendOf(typeof(SFSComponent))]
    public static partial class SFSComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSComponent self)
        {
            self.StartSync = false;
            self.CurrentFrame = 0;
            self.MyRoom = self.GetParent<BattleRoom>();
            self.LocalPlayerId = self.Root().GetComponent<PlayerComponent>().MyId;
        }

        [EntitySystem]
        private static void Update(this SFSComponent self)
        {
            if (!self.StartSync)
                return;
            // Tick
            long timeNow = TimeInfo.Instance.ServerNow();
            if (timeNow < self.ClientUpdate.FrameTime(self.CurrentFrame + 1))
            {
                self.HandleAheadOfFrame();
                return; 
            }
            self.CurrentFrame++;
            self.CurrentArrivedFrame = self.CurrentFrame;
            // Handle Cmd That Server Send
            self.HandleCmdThatServerSend();
            // Send Cmd
            self.MyRoom.GetComponent<PlayerInputComponent>().Tick();
            // 执行玩家输入（预测），并 Tick Component
            self.Tick();
            // Send Message To Server
            self.SendCurrentFrameMessage();
            // Handle TargetAhead
            self.HandleAheadOfFrame();

            // 我们不需要TickView，给view相关的组件写Update即可
        }

        private static void HandleCmdThatServerSend(this SFSComponent self)
        {
            if (self.FrameCmdToHandle.Count == 0)
                return;
            int frame = self.FrameCmdToHandle.First().Key;
            Queue<IRoomCmd> cmds = self.FrameCmdToHandle.First().Value;
            bool shouldRollback = false;
            foreach (var cmd in cmds)
            {
                if (cmd.UnitId != self.LocalPlayerId)
                    self.HandleCmd(cmd);
                else
                {
                    if (!self.CheckConsistencyOnTargetFrame(frame, cmd))
                    {
                        shouldRollback = true;
                        cmd.PassConsistencyCheck = false;
                        Log.Error($"由于{MongoHelper.ToJson(cmd)}的不一致，准备进入回滚流程");
                    }
                    else cmd.PassConsistencyCheck = true;
                }
            }

            if (shouldRollback)
            {
                self.FailCount++;
                self.CurrentFrame = frame;
                foreach (var cmd in cmds)
                {
                    // 本地玩家的的指令才会回滚
                    if (cmd.UnitId == self.LocalPlayerId)
                    {
                        // 回滚处理
                        if (!cmd.PassConsistencyCheck)
                        {
                            self.Rollback(cmd);
                        }
                        cmd.PassConsistencyCheck = true;
                    }
                }
                // And Tick，这一帧结束的数据已经Rollback，从下一帧开始Tick
                self.CurrentFrame++;
                for (; self.CurrentFrame < self.CurrentArrivedFrame; self.CurrentFrame++)
                    self.Tick();
            }

            self.FrameCmdToHandle.Remove(frame);
        }

        private static void Rollback(this SFSComponent self, IRoomCmd cmd)
        {
            SFSUnit unit = self.MyRoom.GetComponent<SFSUnitComponent>().GetChild<SFSUnit>(cmd.UnitId);
            switch (cmd.CmdType)
            {
                case SFSCmdType.MoveCmd:
                    unit.Rollback(cmd as MoveCmd);
                    break;
                default:
                    Log.Error($"CmdType: {cmd.CmdType} Not Found");
                    break;
            }
        }

        private static bool CheckConsistencyOnTargetFrame(this SFSComponent self, int targetFrame, IRoomCmd cmd)
        {
            SFSUnit unit = self.MyRoom.GetComponent<SFSUnitComponent>().GetChild<SFSUnit>(cmd.UnitId);
            switch (cmd.CmdType)
            {
                case SFSCmdType.MoveCmd:
                    return unit.CheckConsistency(targetFrame, cmd as MoveCmd);
                default:
                    Log.Error($"CmdType: {cmd.CmdType} Not Found");
                    return false;
            }
        }
        
        public static void StartSync(this SFSComponent self, long startTime)
        {
            self.StartSync = true;
            self.ClientUpdate = new FixedUpdate(startTime, 0, SFSConstValue.UpdateInterval);
            self.ServerUpdate = new FixedUpdate(startTime, 0, SFSConstValue.UpdateInterval);
        }

        private static void SendCurrentFrameMessage(this SFSComponent self)
        {
            var sender = self.Root().GetComponent<ClientSenderComponent>();
            if (self.FrameCmdToSend.TryGetValue(self.CurrentFrame, out var cmdQueueToSend))
            {
                foreach (var cmdToSend in cmdQueueToSend)
                {
                    sender.Send(cmdToSend);
                }
            }
            self.FrameCmdToSend.Remove(self.CurrentFrame);
        }

        private static void HandleAheadOfFrame(this SFSComponent self)
        {
            int serverFrame = self.GetServerCurrentFrame();
            // 客户端正常领先于服务器， 客户端断线可以在Ping中捕获异常并抛出
            if (self.CurrentFrame > serverFrame)
            {
                self.CurrentAheadOfFrame = self.CurrentFrame - serverFrame;
            }
            else
            {
                self.CurrentAheadOfFrame = self.CurrentFrame - serverFrame;
                int count = self.TargetAheadOfFrame - self.CurrentAheadOfFrame;
                while (count > 0)
                {
                    self.CurrentFrame++;
                    self.Tick();
                    count--;
                }
            }
            
            if (self.CurrentAheadOfFrame != self.TargetAheadOfFrame)
            {
                // Log.Info("------------------进入变速状态");
                self.HasInSpeedChangeState = true;
                int newInterval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond /
                    (SFSConstValue.FrameCountPerSecond +
                        self.TargetAheadOfFrame -
                        self.CurrentAheadOfFrame
                    )).Milliseconds;
                self.ClientUpdate.ChangeInterval(newInterval, self.CurrentFrame);
            }
            else if (self.HasInSpeedChangeState)
            {
                // Log.Info("------------------已经对齐");
                self.HasInSpeedChangeState = false;
                self.ClientUpdate.ChangeInterval(SFSConstValue.UpdateInterval, self.CurrentFrame);
            }
        }

        private static void Tick(this SFSComponent self)
        {
            // Handle Player Input
            if (self.PlayerInputCmdBuffer.TryGetValue(self.CurrentFrame, out Queue<IRoomCmd> cmdQueueToSend))
            {
                foreach (var cmd in cmdQueueToSend)
                {
                    self.HandleCmd(cmd);
                }
            }
            // Tick Component
            self.MyRoom.GetComponent<SFSUnitComponent>().Tick();
        }

        private static void HandleCmd(this SFSComponent self, IRoomCmd cmd)
        {
            SFSUnit unit = self.MyRoom.GetComponent<SFSUnitComponent>().GetChild<SFSUnit>(cmd.UnitId);
            switch (cmd.CmdType)
            {
                case SFSCmdType.MoveCmd:
                    unit.HandleCmd(cmd as MoveCmd);
                    break;
                case SFSCmdType.SkillCmd:
                    unit.GetComponent<SkillComponent>().HandleCmd(cmd as SkillCmd);
                    break;
                default:
                    Log.Error($"CmdType: {cmd.CmdType} Not Found");
                    break;
            }
        }
        
        // 这个是在Tick PlayerInputComponent时调用的，此时CurrentFrame已经更新了
        public static void AddCmdToSendQueue(this SFSComponent self, IRoomCmd cmd, bool shouldAddToPlayerInputBuffer = true)
        {
            int frame = self.CurrentFrame;
            cmd.FrameId = frame;
            if (self.FrameCmdToSend.TryGetValue(frame, out Queue<IRoomCmd> queue))
            {
                queue.Enqueue(cmd);
            }
            else
            {
                Queue<IRoomCmd> newQueue = new Queue<IRoomCmd>();
                newQueue.Enqueue(cmd);
                self.FrameCmdToSend.Add(frame, newQueue);
            }

            if (!shouldAddToPlayerInputBuffer) return;

            if (self.PlayerInputCmdBuffer.TryGetValue(frame, out Queue<IRoomCmd> q))
            {
                q.Enqueue(cmd);
            }
            else
            {
                Queue<IRoomCmd> newQueue = new Queue<IRoomCmd>();
                newQueue.Enqueue(cmd);
                self.PlayerInputCmdBuffer.Add(frame, newQueue);
            }
        }

        public static void ChangePing(this SFSComponent self, long newPing)
        {
            self.HalfRTT = (newPing + 1) >> 1;
            self.TargetAheadOfFrame = TimeAndFrameConverter.Long2Frame(self.HalfRTT) + self.FrameBuffer;
        }

        private static int GetServerCurrentFrame(this SFSComponent self)
        {
            return self.ServerUpdate.GetFrame(TimeInfo.Instance.ServerNow());
        }

        public static void AddCmdToHandleQueue(this SFSComponent self, IRoomCmd cmd)
        {
            int frame = cmd.FrameId;
            if (self.FrameCmdToHandle.TryGetValue(frame, out Queue<IRoomCmd> queue))
            {
                queue.Enqueue(cmd);
            }
            else
            {
                Queue<IRoomCmd> newQueue = new Queue<IRoomCmd>();
                newQueue.Enqueue(cmd);
                self.FrameCmdToHandle.Add(frame, newQueue);
            }
        }
    }
}
