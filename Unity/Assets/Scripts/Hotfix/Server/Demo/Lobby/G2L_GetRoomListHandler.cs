using System.Collections.Generic;

namespace ET.Server
{

    [MessageHandler(SceneType.Lobby)]
    [FriendOf(typeof(SFSRoom))]
    public class G2L_GetRoomListHandler : MessageHandler<Scene, G2L_GetRoomList, L2G_GetRoomList>
    {
        protected override async ETTask Run(Scene root, G2L_GetRoomList request, L2G_GetRoomList response)
        {
            var roomsCmpt = root.GetComponent<SFSRoomsComponent>();
            response.RoomId = new List<long>();
            response.RoomHolderId = new List<long>();
            response.PlayerNum = new List<int>();
            foreach (var entity in roomsCmpt.Children.Values)
            {
                SFSRoom room = entity as SFSRoom;
                if (room == null)
                    continue;
                response.RoomId.Add(room.Id);
                response.RoomHolderId.Add(room.RoomHolderId);
                response.PlayerNum.Add(room.Players.Count);
            }
            await ETTask.CompletedTask;
        }
    }
}
