namespace ET.Server
{
    
    [MessageHandler(SceneType.Lobby)]
    public class G2L_CreateRoomHandler : MessageHandler<Scene, G2L_CreateRoom,L2G_CreateRoom>
    {
        protected override async ETTask Run(Scene root, G2L_CreateRoom request, L2G_CreateRoom response)
        {
            var room = root.GetComponent<SFSRoomsComponent>().AddChild<SFSRoom, long>(request.PlayerId);
            response.RoomId = room.Id;
            await ETTask.CompletedTask;
        }
    }
}
