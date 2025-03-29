using System.Collections.Generic;

namespace ET.Server
{
    [ChildOf]
    public class PlayerGameInfo : Entity, IAwake
    {
        public long PlayerId;
        public List<long> BattleId = new List<long>();
    }
}
