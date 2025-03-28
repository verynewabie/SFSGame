namespace ET.Server
{

    [MessageHandler(SceneType.SFSRoom)]
    [FriendOf(typeof(SFSRoomPlayer))]
    [FriendOfAttribute(typeof(ET.Server.SFSComponent))]
    public class C2Room_ReconnectDoneHandler : MessageHandler<Scene, C2Room_ReconnectDone>
    {
        protected override async ETTask Run(Scene root, C2Room_ReconnectDone message)
        {
            BattleRoom room = root.GetComponent<BattleRoom>();
            SFSRoomPlayerComponent component = room.GetComponent<SFSRoomPlayerComponent>();
            component.SetPlayerOnline(message.PlayerId);

            Room2C_ReconnectEnterGame response = Room2C_ReconnectEnterGame.Create();
            SFSComponent tickCmpt = room.GetComponent<SFSComponent>();

            room.GetComponent<SFSComponent>().SyncAllCmd(message.PlayerId, component.GetReconnectStartFrame(message.PlayerId) + 1, 
                tickCmpt.CurrentFrame);
            response.StartTime = tickCmpt.FixedUpdate.StartTime;
            response.Frame = tickCmpt.FixedUpdate.StartFrame;
            room.SendToPlayer(response, message.PlayerId);
            await ETTask.CompletedTask;
        }
    }
}