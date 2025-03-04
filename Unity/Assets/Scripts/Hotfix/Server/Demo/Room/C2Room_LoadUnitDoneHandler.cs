namespace ET.Server
{

    [MessageHandler(SceneType.SFSRoom)]
    [FriendOfAttribute(typeof(ET.Server.SFSRoomPlayer))]
    public class C2Room_LoadUnitDoneHandler : MessageHandler<Scene, C2Room_LoadGameDone>
    {
        protected override async ETTask Run(Scene root, C2Room_LoadGameDone message)
        {
            BattleRoom room = root.GetComponent<BattleRoom>();
            SFSRoomPlayerComponent component = room.GetComponent<SFSRoomPlayerComponent>();
            component.SetPlayerReady(message.PlayerId);
            if (component.IsAllReady())
            {
                Room2C_SFSEnterGame room2C_SFSEnterGame = Room2C_SFSEnterGame.Create();
                room.Broadcast(room2C_SFSEnterGame);
            }
            await ETTask.CompletedTask;
        }
    }
}
