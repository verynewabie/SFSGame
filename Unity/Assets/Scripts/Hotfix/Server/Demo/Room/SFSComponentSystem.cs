using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{

    [EntitySystemOf(typeof(SFSComponent))]
    [FriendOf(typeof(SFSComponent))]
    [FriendOf(typeof(BattleRoom))]
    [FriendOf(typeof(PlayerGameInfo))]
    [FriendOf(typeof(OneGameInfo))]
    [FriendOf(typeof(SFSUnit))]
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
            // TickEnd
            self.MyRoom.GetComponent<SFSUnitComponent>().TickEnd();
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

            // Check If Game Ends
            if (self.MyRoom.GetComponent<SFSUnitComponent>().IsBlueAllDie())
            {
                self.EndGame(SFSUnitCamp.Red).Coroutine();
            }
            else if (self.MyRoom.GetComponent<SFSUnitComponent>().IsRedAllDie())
            {
                self.EndGame(SFSUnitCamp.Blue).Coroutine();
            }
        }

        private static async ETTask EndGame(this SFSComponent self, SFSUnitCamp winCamp)
        {
            self.StartSync = false;
            Room2C_GameEnd msg = Room2C_GameEnd.Create();
            msg.WinCamp = winCamp;
            self.MyRoom.Broadcast(msg);

            // Save DataBase
            List<long> playerIds = self.MyRoom.PlayerId;
            DBComponent dbComponent = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(1);
            long battleId = self.MyRoom.InstanceId;
            SFSUnitComponent unitComponent = self.MyRoom.GetComponent<SFSUnitComponent>();
            foreach (long playerId in playerIds)
            {
                SFSUnit unit = unitComponent.GetChild<SFSUnit>(playerId);
                BattleInfo battleInfo = BattleInfo.Create();
                battleInfo.BattleId = battleId;
                battleInfo.Win = winCamp == unit.SfsUnitCamp;
                battleInfo.Time = self.FixedUpdate.StartTime;
                await dbComponent.AddPlayerBattle(playerId, battleInfo);
            }

            OneGameInfo oneGameInfo = self.AddChild<OneGameInfo>();
            oneGameInfo.BattleId = battleId;
            for (int i = 0; i < playerIds.Count; i++)
            {
                long id = playerIds[i];
                SFSUnitInfo info = SFSUnitInfo.Create();
                info.UnitId = id;
                info.Camp = i * 2 < playerIds.Count ? SFSUnitCamp.Red : SFSUnitCamp.Blue;
                info.Position = info.Camp == SFSUnitCamp.Red ? new float3(-5f, 0f, 0f) : new float3(5f, 0f, 0f);
                info.Forward = quaternion.identity;
                info.Type = SFSUnitType.Player;
                info.State = SFSUnitState.Free;
                oneGameInfo.Units.Add(info);
            }

            foreach ((int key, Queue<IRoomCmd> value) in self.WholeCmds)
            {
                oneGameInfo.Cmds.Add(key.ToString(), value);
            }
            await dbComponent.Save(oneGameInfo);

            // Delete Room
            await self.Root().GetComponent<TimerComponent>().WaitAsync(5000);
            FiberManager.Instance.Remove(self.Fiber().Id).Coroutine();
        }

        public static void AddCmdToSendQueue(this SFSComponent self, IRoomCmd cmd)
        {
            int frame = self.CurrentFrame;
            cmd.FrameId = frame;
            self.AddCmdToWholeBuffer(cmd);
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

        private static void AddCmdToWholeBuffer(this SFSComponent self, IRoomCmd cmd)
        {
            int frame = cmd.FrameId;
            if (self.WholeCmds.TryGetValue(frame, out Queue<IRoomCmd> queue))
            {
                queue.Enqueue(cmd);
            }
            else
            {
                Queue<IRoomCmd> newQueue = new Queue<IRoomCmd>();
                newQueue.Enqueue(cmd);
                self.WholeCmds.Add(frame, newQueue);
            }
        }

        public static void AddCmdToHandleQueue(this SFSComponent self, IRoomCmd cmd)
        {
            int frame = cmd.FrameId;
            if (frame <= self.CurrentFrame)
            {
                Log.Error($"Receive Cmd: {cmd.CmdType} {cmd.FrameId}\n in Frame:{self.CurrentFrame}");
                cmd.FrameId = frame = self.CurrentFrame + 1;
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

        public static void SyncAllCmd(this SFSComponent self, long unitId, int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                if (self.WholeCmds.TryGetValue(i, out Queue<IRoomCmd> queue))
                {
                    foreach (var cmd in queue)
                    {
                        self.MyRoom.SendToPlayer(cmd, unitId);
                    }
                }
            }
        }
    }
}
