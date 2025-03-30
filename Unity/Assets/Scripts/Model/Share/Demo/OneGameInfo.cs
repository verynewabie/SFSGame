using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ChildOf]
    public class OneGameInfo : Entity, IAwake
    {
        public long BattleId;
        public List<SFSUnitInfo> Units = new List<SFSUnitInfo>();
        public Dictionary<string, Queue<IRoomCmd>> Cmds = new();
    }
}
