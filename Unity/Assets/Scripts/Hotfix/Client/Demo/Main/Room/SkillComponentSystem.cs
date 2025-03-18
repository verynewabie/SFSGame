namespace ET.Client
{

    [EntitySystemOf(typeof(SkillComponent))]
    public static partial class SkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SkillComponent self, SFSUnit owner)
        {
            self.Owner = owner;
        }

        public static void HandleCmd(this SkillComponent self, SkillCmd cmd)
        {
            if (cmd.IsReleaseCmd && self.Owner.CanReleaseSkill())
            {
                // TODO Play Animation
            }
            
        }
    }
}
