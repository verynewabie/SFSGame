using System.Collections.Generic;

namespace ET.Server
{
    
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_EnterRoomHandler : MessageSessionHandler<C2G_EnterRoom, G2C_EnterRoom>
    {
        protected override async ETTask Run(Session session, C2G_EnterRoom request, G2C_EnterRoom response)
        {
            StartSceneConfig lobbyConfig = AddressHelper.GetLobby(session.Zone());
            var players = session.Fiber().Root.GetComponent<PlayerComponent>();
            G2L_EnterRoom g2L_EnterRoom = G2L_EnterRoom.Create();
            g2L_EnterRoom.PlayerId = request.PlayerId;
            g2L_EnterRoom.RoomId = request.RoomId;
            g2L_EnterRoom.Name = players.GetChild<Player>(request.PlayerId).Account;
            L2G_EnterRoom l2G_EnterRoom = await session.Fiber().Root.GetComponent<MessageSender>().Call(
                lobbyConfig.ActorId, g2L_EnterRoom) as L2G_EnterRoom;
            response.RoomHolderId = l2G_EnterRoom.RoomHolderId;
            response.PlayerId = l2G_EnterRoom.PlayerId;
            response.PlayerName = new List<string>();
            
            foreach (long id in response.PlayerId)
            {
                var player = players.GetChild<Player>(id);
                response.PlayerName.Add(player.Account);
            }
        }
    }
}
