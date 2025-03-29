using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ET.Server
{
    [ChildOf]
    public class OneGameInfo : Entity, IAwake
    {
        public long BattleId;
        public List<SFSUnitInfo> Units = new List<SFSUnitInfo>();
        public Dictionary<string, Queue<IRoomCmd>> Cmds = new();
    }
}
