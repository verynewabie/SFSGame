namespace ET.Server
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

        public UnitSkillState State;
        /// <summary>
        /// 持续时间，单位是帧
        /// </summary>
        public int Duration;
    }
}
