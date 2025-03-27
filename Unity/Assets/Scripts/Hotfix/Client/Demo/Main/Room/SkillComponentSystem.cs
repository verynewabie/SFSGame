namespace ET.Client
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

        public static void HandleCmd(this SkillComponent self, SkillCmd cmd)
        {
            if (cmd.FromClient && self.Owner.CanReleaseSkill && self.State == SFSSkillState.None)
            {
                self.State = SFSSkillState.Forward;
                self.Duration = SFSConstValue.SkillForward;
                // 不用在这里播动画，动画组件会一直读State
            }
            else if (!cmd.FromClient)
            {
                self.State = cmd.State;
                self.Duration = cmd.Duration;
                // 进入CD，说明施法成功了
                if (cmd.State == SFSSkillState.CD)
                {
                    SFSUnitFactory.CreateProjectile(self.Owner.BattleRoom, cmd.Info);
                }
            }
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
            // 客户端不会处理技能生效，而是等服务器
        }

        public static void TickEnd(this SkillComponent self)
        {
            SkillCmd cmd = SkillCmd.Create();
            cmd.State = self.State;
            cmd.Duration = self.Duration;
            cmd.UnitId = self.Owner.Id;
            cmd.CmdType = SFSCmdType.SkillCmd;

            var sfsCmpt = self.Owner.BattleRoom.GetComponent<SFSComponent>();
            self.HistorySkillState[sfsCmpt.CurrentFrame] = cmd;
            cmd.FrameId = sfsCmpt.CurrentFrame;
        }
        
        public static bool CheckConsistency(this SkillComponent self, int frame, SkillCmd skillCmd)
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

        public static void Rollback(this SkillComponent self, SkillCmd cmd)
        {
            // 客户端在Forward，而服务器已经回到None
            if (cmd.State == SFSSkillState.None)
            {
                self.State = cmd.State;
                self.Duration = cmd.Duration;
            }
            // 客户端在Forward，服务器已经成功施法，到了CD
            else if (cmd.State == SFSSkillState.CD)
            {
                self.State = cmd.State;
                self.Duration = cmd.Duration;
            }
            else
            {
                Log.Error($"Local State:{self.State.ToString()}, Cmd State:{cmd.State.ToString()}");
            }
        }
    }
}
