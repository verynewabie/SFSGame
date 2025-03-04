using System.Collections.Generic;

namespace ET
{

    [ComponentOf(typeof(Scene))]
    public class BattleRoom : Entity, IAwake<List<long>>
    {
        public List<long> PlayerId;
    }
}
