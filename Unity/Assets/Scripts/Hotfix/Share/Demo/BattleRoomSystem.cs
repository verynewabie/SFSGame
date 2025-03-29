using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(BattleRoom))]
    [FriendOf(typeof(BattleRoom))]
    public static partial class BattleRoomSystem
    {
        [EntitySystem]
        private static void Awake(this BattleRoom self, List<long> players)
        {
            self.PlayerId = players;
        }
    }
}
