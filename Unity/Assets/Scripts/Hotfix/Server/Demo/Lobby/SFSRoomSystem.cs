using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(SFSRoom))]
    [FriendOf(typeof(SFSRoom))]
    public static partial class SFSRoomSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.SFSRoom self, long roomHolderId)
        {
            self.Players = new List<long>();
            self.RoomHolderId = roomHolderId;
            self.Players.Add(roomHolderId);
        }

        public static void EnterRoom(this ET.Server.SFSRoom self, long playerId)
        {
            self.Players.Add(playerId);
        }

        public static void Broadcast(this ET.Server.SFSRoom self, IMessage message)
        {
            // 广播的消息不能被池回收
            (message as MessageObject).IsFromPool = false;
            
            MessageLocationSenderComponent sender = self.Root().GetComponent<MessageLocationSenderComponent>();
            foreach (long playerId in self.Players)
            {
                sender.Get(LocationType.GateSession).Send(playerId, message);
            }
        }
        
    }
}