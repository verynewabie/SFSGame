namespace ET.Client
{

    [ComponentOf(typeof(SFSUnit))]
    public class SkillComponent : Entity, IAwake<SFSUnit>
    {
        private EntityRef<SFSUnit> owner;
        public SFSUnit Owner
        {
            get => owner;
            set => owner = value;
        }
    }
}
