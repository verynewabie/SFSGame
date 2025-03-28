namespace ET.Server
{
    [MessageHandler(SceneType.SFSRoom)]
    [FriendOf(typeof(SFSRoomPlayer))]
    [FriendOfAttribute(typeof(ET.Server.SFSComponent))]
    public class G2Room_PlayerReconnectHandler : MessageHandler<Scene, G2Room_PlayerReconnect>
    {
        protected override async ETTask Run(Scene root, G2Room_PlayerReconnect message)
        {
            var room = root.GetComponent<BattleRoom>();
            var info = room.GetComponent<SFSUnitComponent>().Reconnect();
            SFSComponent tickCmpt = room.GetComponent<SFSComponent>();
            SFSRoomPlayerComponent component = room.GetComponent<SFSRoomPlayerComponent>();
            component.SetReconnectStartFrame(message.PlayerId, tickCmpt.CurrentFrame);
            room.SendToPlayer(info, message.PlayerId);
            await ETTask.CompletedTask;
        }
    }
}