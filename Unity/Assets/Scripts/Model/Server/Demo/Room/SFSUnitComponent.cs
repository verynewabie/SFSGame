using System.Collections.Generic;

namespace ET.Server
{
    public struct ProjectileInfo
    {
        public SFSUnitInfo Info;
        public SFSUnit BelongToUnit;
    }
    
    [ComponentOf(typeof(BattleRoom))]
    public class SFSUnitComponent :Entity, IAwake
    {
        public List<ProjectileInfo> unitToCreate = new List<ProjectileInfo>();
        public List<long> unitToDelete = new List<long>();
    }
}