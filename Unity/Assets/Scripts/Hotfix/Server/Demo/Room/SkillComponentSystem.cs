using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(SkillComponent))]
    [FriendOf(typeof(SkillComponent))]
    [FriendOf(typeof(SFSComponent))]
    [FriendOf(typeof(SFSUnit))]
    public static partial class SkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SkillComponent self, SFSUnit owner)
        {
            self.Owner = owner;
            self.State = SFSSkillState.None;
            self.Duration = -1;
        }

        public static void Tick(this SkillComponent self)
        {
            if (self.State == SFSSkillState.None)
                return;
            if (self.State == SFSSkillState.CD)
            {
                self.Duration--;
                if (self.Duration == 0)
                {
                    self.State = SFSSkillState.None;
                    self.Duration = -1;
                }
                return;
            }
            if (!self.Owner.CanReleaseSkill)
            {
                self.State = SFSSkillState.None;
                self.Duration = -1;
                return;
            }
            self.Duration--;
            if (self.Duration == 0)
            {
                // 技能生效
                SFSUnitInfo info = SFSUnitInfo.Create();
                info.Camp = self.Owner.SfsUnitCamp;
                info.Forward = self.Owner.Rotation;
                info.Position = self.Owner.Position;
                info.Type = SFSUnitType.Projectile;
                info.UnitId = IdGenerater.Instance.GenerateInstanceId();
                float3 forward = math.normalize(math.forward(info.Forward));
                float3 right = math.normalize(math.cross(math.up(), forward));
                info.Position += forward * 0.5f + right * 0.5f + math.up();
                self.ToCreateProjectile = info;
                EventSystem.Instance.Publish(self.Root(), new AddUnitCreateInfo
                {
                    Info = info,
                    BelongToUnit = self.Owner
                });
                // self.Owner.BattleRoom.GetComponent<SFSUnitComponent>().AddUnitToCreate(info);
                self.State = SFSSkillState.CD;
                self.Duration = SFSConstValue.SkillCD;
            }
        }

        public static void TickEnd(this SkillComponent self)
        {
            // if player dies, state change to none
            if (self.Owner.SfsUnitState == SFSUnitState.Die)
            {
                self.State = SFSSkillState.None;
                self.Duration = -1;
            }
            
            var sfsCmpt = self.Owner.BattleRoom.GetComponent<SFSComponent>();
            
            SkillCmd cmd = SkillCmd.Create();
            cmd.State = self.State;
            cmd.Duration = self.Duration;
            cmd.UnitId = self.Owner.Id;
            cmd.CmdType = SFSCmdType.SkillCmd;
            cmd.Info = self.ToCreateProjectile;
            cmd.FrameId = sfsCmpt.CurrentFrame;
            
            self.HistorySkillState[sfsCmpt.CurrentFrame] = cmd;
            
            if (!self.CheckConsistency(sfsCmpt.CurrentFrame - 1, cmd))
            {
                EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
                {
                    Cmd = cmd,
                });
            }
        }

        public static void HandleCmd(this SkillComponent self, SkillCmd cmd)
        {
            if (self.Owner.CanReleaseSkill && self.State == SFSSkillState.None)
            {
                self.State = SFSSkillState.Forward;
                self.Duration = SFSConstValue.SkillForward;
            }
        }

        private static bool CheckConsistency(this SkillComponent self, int frame, SkillCmd skillCmd)
        {
            if (!self.HistorySkillState.ContainsKey(frame))
                return false;
            SkillCmd target = self.HistorySkillState[frame];
            if (target.State != skillCmd.State)
                return false;
            if (target.State == SFSSkillState.None)
                return true;
            return target.FrameId - skillCmd.FrameId == skillCmd.Duration - target.Duration;
        }
    }
}
