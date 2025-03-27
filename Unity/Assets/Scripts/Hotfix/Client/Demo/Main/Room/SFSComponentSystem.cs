using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

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
                        Log.Error($"In Frame {frame} rollback because {cmd.CmdType}");
                        shouldRollback = true;
                        cmd.PassConsistencyCheck = false;
                    }
                    else cmd.PassConsistencyCheck = true;
                }
            }

            if (shouldRollback)
            {
                self.IsInChaseFrameState = true;
                self.FailCount++;
                self.CurrentFrame = frame;
                foreach (var cmd in cmds)
                {
                    // 本地玩家的的指令才会回滚
                    if (cmd.UnitId == self.LocalPlayerId)
                    {
                        self.Rollback(cmd);
                        // 回滚处理
                        if (!cmd.PassConsistencyCheck)
                        {
                            self.HandleCmd(cmd);
                        }
                        cmd.PassConsistencyCheck = true;
                    }
                }
                // And Tick，这一帧结束的数据已经Rollback，从下一帧开始Tick
                self.CurrentFrame++;
                for (; self.CurrentFrame < self.CurrentArrivedFrame; self.CurrentFrame++)
                    self.Tick();
                self.IsInChaseFrameState = false;
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
                case SFSCmdType.SkillCmd:
                    unit.GetComponent<SkillComponent>().Rollback(cmd as SkillCmd);
                    break;
                case SFSCmdType.StateCmd:
                    unit.Rollback(cmd as StateCmd);
                    break;
                case SFSCmdType.AttributeCmd:
                    unit.Rollback(cmd as AttributeCmd);
                    break;
                case SFSCmdType.BuffCmd:
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
                case SFSCmdType.SkillCmd:
                    return unit.GetComponent<SkillComponent>().CheckConsistency(targetFrame, cmd as SkillCmd);
                case SFSCmdType.StateCmd:
                    return unit.CheckConsistency(targetFrame, cmd as StateCmd);
                case SFSCmdType.AttributeCmd:
                    return unit.CheckConsistency(targetFrame, cmd as AttributeCmd);
                case SFSCmdType.BuffCmd:
                    return true;
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
                int newSecondFrame = SFSConstValue.FrameCountPerSecond +
                        self.TargetAheadOfFrame -
                        self.CurrentAheadOfFrame;
                int limit = SFSConstValue.FrameCountPerSecond / 10;
                if (newSecondFrame > SFSConstValue.FrameCountPerSecond - limit
                    && newSecondFrame < SFSConstValue.FrameCountPerSecond)
                    newSecondFrame = SFSConstValue.FrameCountPerSecond - limit;
                if (newSecondFrame < SFSConstValue.FrameCountPerSecond + limit
                    && newSecondFrame > SFSConstValue.FrameCountPerSecond)
                    newSecondFrame = SFSConstValue.FrameCountPerSecond + limit;
                newSecondFrame = math.clamp(newSecondFrame, 
                    SFSConstValue.FrameCountPerSecond * 3 / 4,
                    SFSConstValue.FrameCountPerSecond * 5 / 4);
                int newInterval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond /
                    newSecondFrame).Milliseconds;
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
            self.MyRoom.GetComponent<SFSUnitComponent>().Tick(self.IsInChaseFrameState);
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
                case SFSCmdType.DeleteUnitCmd:
                {
                    Room2C_DeleteUnit msg = cmd as Room2C_DeleteUnit;
                    var unitCmpt = self.MyRoom.GetComponent<SFSUnitComponent>();
                    foreach (var id in msg.UnitToDelete)
                    {
                        unitCmpt.RemoveChild(id);
                    }
                    EventSystem.Instance.Publish(self.Root(), new RemoveUnitView
                    {
                        UnitToDelete = msg.UnitToDelete
                    });
                }
                    break;
                case SFSCmdType.DebugInfoCmd:
                {
                    Room2C_DebugInfo msg = cmd as Room2C_DebugInfo;
                    EventSystem.Instance.Publish(self.Root(), new ShowDebugInfo
                    {
                        Pos = msg.Pos,
                        Radius = msg.Radius,
                    });
                }
                    break;
                case SFSCmdType.StateCmd:
                    unit.HandleCmd(cmd as StateCmd);
                    break;
                case SFSCmdType.AttributeCmd:
                    unit.HandleCmd(cmd as AttributeCmd);
                    break;
                case SFSCmdType.BuffCmd:
                    unit.GetComponent<BuffComponent>().HandleCmd(cmd as BuffCmd);
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
            self.CacheCmdToHandle.Enqueue(cmd);
        }

        public static void OneFrameEndHandler(this SFSComponent self, Room2C_OneFrameEnd msg)
        {
            Queue<IRoomCmd> newQueue = new Queue<IRoomCmd>();
            foreach (var cmd in self.CacheCmdToHandle)
            {
                newQueue.Enqueue(cmd);
            }
            self.FrameCmdToHandle.Add(msg.FrameId, newQueue);
            self.CacheCmdToHandle.Clear();
        }
    }
}
