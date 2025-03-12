using System.Collections.Generic;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSComponent))]
    [FriendOfAttribute(typeof(ET.Client.SFSComponent))]
    public static partial class SFSComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSComponent self)
        {
            self.StartSync = false;
            self.CurrentFrame = 0;
            self.MyRoom = self.GetParent<BattleRoom>();
        }

        [EntitySystem]
        private static void Update(this SFSComponent self)
        {
            if (!self.StartSync)
                return;
            // Tick
            long timeNow = TimeInfo.Instance.ServerNow();
            if (timeNow < self.ClientUpdate.FrameTime(self.CurrentFrame + 1))
                return; 
            self.CurrentFrame++;
            self.CurrentArrivedFrame = self.CurrentFrame;
            // Handle Cmd That Server Send TODO
            
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
    }
}
