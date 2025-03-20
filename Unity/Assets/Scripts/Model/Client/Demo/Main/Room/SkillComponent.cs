using System.Collections.Generic;

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
        
        public SFSSkillState State;

        /// <summary>
        /// 单位：帧
        /// </summary>
        public int Duration;
        
        public Dictionary<int, SkillCmd> HistorySkillState = new Dictionary<int, SkillCmd>();
    }
}
