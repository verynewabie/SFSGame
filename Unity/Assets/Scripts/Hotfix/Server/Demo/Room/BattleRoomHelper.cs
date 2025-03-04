namespace ET.Server
{
    [FriendOfAttribute(typeof(ET.Server.SFSRoomPlayer))]
    public static class BattleRoomHelper
    {
        public static void Broadcast(this BattleRoom room, IMessage message)
        {
            // 广播的消息不能被池回收
            (message as MessageObject).IsFromPool = false;

            SFSRoomPlayerComponent roomPlayerComponent = room.GetComponent<SFSRoomPlayerComponent>();

            MessageLocationSenderComponent messageLocationSenderComponent = room.Root().GetComponent<MessageLocationSenderComponent>();
            foreach (var kv in roomPlayerComponent.Children)
            {
                SFSRoomPlayer roomPlayer = kv.Value as SFSRoomPlayer;

                if (!roomPlayer.IsOnline)
                {
                    continue;
                }
                Log.Info($"Send {message.GetType().ToString()} Message To {roomPlayer.Id}");
                messageLocationSenderComponent.Get(LocationType.GateSession).Send(roomPlayer.Id, message);
            }
        }
    }
}
