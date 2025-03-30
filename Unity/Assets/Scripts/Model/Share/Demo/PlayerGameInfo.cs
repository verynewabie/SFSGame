using System.Collections.Generic;

namespace ET
{
    [ChildOf]
    public class PlayerGameInfo : Entity, IAwake
    {
        public long PlayerId;
        public List<BattleInfo> Battles = new List<BattleInfo>();
    }
}
