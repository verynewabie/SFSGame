using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(SFSUnit))]
    public class BuffComponent : Entity, IAwake<SFSUnit>
    {
        public List<SFSBuff> Buff = new();
        
        private EntityRef<SFSUnit> owner;
        public SFSUnit Owner
        {
            get => owner;
            set => owner = value;
        }
    }
}
