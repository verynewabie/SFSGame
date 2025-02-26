using System.Collections.Generic;

namespace ET.Server
{

    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_GetRoomListHandler : MessageSessionHandler<C2G_GetRoomList, G2C_GetRoomList>
    {
        protected override async ETTask Run(Session session, C2G_GetRoomList request, G2C_GetRoomList response)
        {
            StartSceneConfig lobbyConfig = AddressHelper.GetLobby(session.Zone());
            G2L_GetRoomList g2LGetRoomList = G2L_GetRoomList.Create();
            L2G_GetRoomList l2GGetRoomList = await session.Fiber().Root.GetComponent<MessageSender>().Call(
                lobbyConfig.ActorId, g2LGetRoomList) as L2G_GetRoomList;
            response.RoomId = new List<long>();
            response.RoomHolderName = new List<string>();
            response.PlayerNum = new List<int>();

            int count = l2GGetRoomList.RoomId.Count;
            var playerCmpt = session.Fiber().Root.GetComponent<PlayerComponent>();
            for (int i = 0; i < count; i++)
            {
                response.RoomId.Add(l2GGetRoomList.RoomId[i]);
                response.PlayerNum.Add(l2GGetRoomList.PlayerNum[i]);
                response.RoomHolderName.Add(playerCmpt.GetChild<Player>(l2GGetRoomList.RoomHolderId[i]).Account);
            }
        }
    }
}
