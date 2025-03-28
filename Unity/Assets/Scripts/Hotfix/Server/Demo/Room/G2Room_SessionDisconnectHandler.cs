namespace ET.Server
{
    [MessageLocationHandler(SceneType.SFSRoom)]
    [FriendOf(typeof(SFSRoomPlayer))]
    public class G2Room_SessionDisconnectHandler : MessageLocationHandler<SFSUnit, G2Room_SessionDisconnect>
    {
        protected override async ETTask Run(SFSUnit unit, G2Room_SessionDisconnect message)
        {
            var roomPlayer = unit.BattleRoom.GetComponent<SFSRoomPlayerComponent>().GetChild<SFSRoomPlayer>(unit.Id);
            roomPlayer.IsOnline = false;
            await ETTask.CompletedTask;
        }
    }
}
