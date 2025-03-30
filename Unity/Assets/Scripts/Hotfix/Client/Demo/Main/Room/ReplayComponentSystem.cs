using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(ReplayComponent))]
    [FriendOf(typeof(ReplayComponent))]
    public static partial class ReplayComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ReplayComponent self)
        {
            self.StartSync = false;
            self.CurrentFrame = 0;
            self.MyRoom = self.GetParent<BattleRoom>();
        }

        [EntitySystem]
        private static void Update(this ReplayComponent self)
        {
            if (!self.StartSync)
                return;
            // Tick
            long timeNow = TimeInfo.Instance.ClientNow();
            if (timeNow < self.FixedUpdate.FrameTime(self.CurrentFrame + 1))
                return; 
            self.CurrentFrame++;
            if (self.CurrentFrame > self.LastFrame)
            {
                EventSystem.Instance.PublishAsync(self.Root(), new ShowUIHint
                {
                    hint = "录像播放结束"
                }).Coroutine();
                return;
            } 
            self.HandleCmdOnTargetFrame(self.CurrentFrame);
            self.Tick();
        }

        private static void Tick(this ReplayComponent self)
        {
            self.MyRoom.GetComponent<SFSUnitComponent>().OnlyTick();
        }

        private static void HandleCmdOnTargetFrame(this ReplayComponent self, int frame)
        {
            if (self.FrameCmdToHandle.TryGetValue(frame, out Queue<IRoomCmd> cmds))
            {
                foreach (var cmd in cmds)
                {
                    self.HandleCmd(cmd);
                }
            }
        }
        
        public static void StartSync(this ReplayComponent self, long startTime)
        {
            self.StartSync = true;
            self.FixedUpdate = new FixedUpdate(startTime, 0, SFSConstValue.UpdateInterval);
        }
        
        private static void HandleCmd(this ReplayComponent self, IRoomCmd cmd)
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

        public static void AddCmdToHandleQueue(this ReplayComponent self, IRoomCmd cmd)
        {
            int frame = cmd.FrameId;    
            self.LastFrame = Math.Max(frame, self.LastFrame);
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
