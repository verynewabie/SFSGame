namespace ET.Server
{

    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_CreateRoomHandler : MessageSessionHandler<C2G_CreateRoom, G2C_CreateRoom>
    {
        protected override async ETTask Run(Session session, C2G_CreateRoom request, G2C_CreateRoom response)
        {
            StartSceneConfig lobbyConfig = AddressHelper.GetLobby(session.Zone());
            var g2LCreateRoom = G2L_CreateRoom.Create();
            g2LCreateRoom.PlayerId = request.PlayerId;
            L2G_CreateRoom l2GCreateRoom = await session.Fiber().Root.GetComponent<MessageSender>().Call(
                lobbyConfig.ActorId, g2LCreateRoom) as L2G_CreateRoom;
            response.RoomId = l2GCreateRoom.RoomId;
        }
    }
}
