using System.Net.NetworkInformation;

namespace ET.Server
{

    [EntitySystemOf(typeof(SFSRoomPlayerComponent))]
    [FriendOfAttribute(typeof(ET.Server.SFSRoomPlayer))]
    public static partial class SFSRoomPlayerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.SFSRoomPlayerComponent self)
        {

        }

        public static void SetPlayerReady(this SFSRoomPlayerComponent self, long playerId)
        {
            self.GetChild<SFSRoomPlayer>(playerId).IsReady = true;
        }

        public static bool IsAllReady(this SFSRoomPlayerComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                SFSRoomPlayer player = entity as SFSRoomPlayer;
                if (player == null)
                    continue;
                if (player.IsReady == false)
                    return false;
            }
            return true;
        }
    }
}
