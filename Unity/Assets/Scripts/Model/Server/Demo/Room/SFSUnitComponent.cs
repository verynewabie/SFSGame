using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(BattleRoom))]
    public class SFSUnitComponent :Entity, IAwake
    {
        public List<SFSUnitInfo> unitToCreate = new List<SFSUnitInfo>();
        public List<long> unitToDelete = new List<long>();
    }
}