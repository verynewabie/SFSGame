namespace ET.Server
{
    

    [MessageHandler(SceneType.Lobby)]
    [FriendOf(typeof(SFSRoom))]
    public class G2L_EnterRoomHandler : MessageHandler<Scene, G2L_EnterRoom, L2G_EnterRoom>
    {
        protected override async ETTask Run(Scene root, G2L_EnterRoom request, L2G_EnterRoom response)
        {
            var room = root.GetComponent<SFSRoomsComponent>().GetChild<SFSRoom>(request.RoomId);
            var newPlayer = L2C_NewPlayer.Create();
            newPlayer.Name = request.Name;
            newPlayer.PlayerId = request.PlayerId;
            room.Broadcast(newPlayer);
            
            room.EnterRoom(request.PlayerId);
            response.RoomHolderId = room.RoomHolderId;
            response.PlayerId = room.Players;
            await ETTask.CompletedTask;
        }
    }
}
