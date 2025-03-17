namespace ET.Server
{

    [MessageHandler(SceneType.SFSRoom)]
    [FriendOf(typeof(SFSRoomPlayer))]
    public class C2Room_LoadGameDoneHandler : MessageHandler<Scene, C2Room_LoadGameDone>
    {
        protected override async ETTask Run(Scene root, C2Room_LoadGameDone message)
        {
            BattleRoom room = root.GetComponent<BattleRoom>();
            SFSRoomPlayerComponent component = room.GetComponent<SFSRoomPlayerComponent>();
            component.SetPlayerReady(message.PlayerId);
            if (!component.IsAllReady())
                return;
            Room2C_SFSEnterGame room2C_SFSEnterGame = Room2C_SFSEnterGame.Create();
            room2C_SFSEnterGame.StartTime = TimeInfo.Instance.ServerFrameTime();
            room.Broadcast(room2C_SFSEnterGame);
            room.AddComponent<SFSComponent>().StartSync(room2C_SFSEnterGame.StartTime);
            await ETTask.CompletedTask;
        }
    }
}
