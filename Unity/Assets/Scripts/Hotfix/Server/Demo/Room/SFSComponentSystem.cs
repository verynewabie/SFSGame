

using System.Collections.Generic;

namespace ET.Server
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
        }
        [EntitySystem]
        private static void Update(this SFSComponent self)
        {
            if (!self.StartSync)
                return;
            long timeNow = TimeInfo.Instance.ServerNow();
            if (timeNow < self.FixedUpdate.FrameTime(self.CurrentFrame + 1))
                return;
            self.CurrentFrame++;
            // Handle Cmd
            if (self.FrameCmdToHandle.TryGetValue(self.CurrentFrame, out Queue<IRoomCmd> queue))
            {
                foreach (var cmd in queue)
                {
                    self.HandleCmd(cmd);
                }
                self.FrameCmdToHandle.Remove(self.CurrentFrame);
            }
            // Tick
            self.Tick();
            // Send Cmd
            self.SendCurrentFrameMessage();
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

        private static void Tick(this SFSComponent self)
        {
            // TickComponent
            self.MyRoom.GetComponent<SFSUnitComponent>().Tick();
            // 这里 Tick 时会删除Unit（添加到队列）
            self.MyRoom.GetComponent<PhysicsWorldComponent>().Tick();
        }
        
        public static void StartSync(this SFSComponent self, long startTime)
        {
            self.StartSync = true;
            self.FixedUpdate = new FixedUpdate(startTime, 0, SFSConstValue.UpdateInterval);
        }

        private static void SendCurrentFrameMessage(this SFSComponent self)
        {
            if (self.FrameCmdToSend.TryGetValue(self.CurrentFrame, out var cmdQueueToSend))
            {
                foreach (var cmdToSend in cmdQueueToSend)
                {
                    self.MyRoom.Broadcast(cmdToSend);
                }
            }
            self.FrameCmdToSend.Remove(self.CurrentFrame);
        }
        
        public static void AddCmdToSendQueue(this SFSComponent self, IRoomCmd cmd)
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
        }

        public static void AddCmdToHandleQueue(this SFSComponent self, IRoomCmd cmd)
        {
            int frame = cmd.FrameId;
            if (frame <= self.CurrentFrame)
            {
                Log.Warning($"Receive Cmd: {cmd.CmdType} {cmd.FrameId}\n in Frame:{self.CurrentFrame}");
            }
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
