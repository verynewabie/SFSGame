namespace ET.Server
{
    [MessageHandler(SceneType.Lobby)]
    [FriendOfAttribute(typeof(ET.Server.SFSRoom))]
    public class C2L_StartGameHandler : MessageHandler<Scene, C2L_StartGame>
    {
        protected override async ETTask Run(Scene root, C2L_StartGame message)
        {
            await ETTask.CompletedTask;
            SFSRoom room = root.GetComponent<SFSRoomsComponent>().GetChild<SFSRoom>(message.RoomId);
            if (room.Players.Count == 1 || room.Players.Count % 2 == 0)
            {
                // 与Map服务器通信，让其创建Unit
                L2M_CreateBattleRoom request = L2M_CreateBattleRoom.Create();
                request.PlayerId.AddRange(room.Players);
                StartSceneConfig startSceneConfig = RandomGenerator.RandomArray(StartSceneConfigCategory.Instance.Maps);
                // 创建BattleRoom后Init时会给客户端发加载游戏的网络消息
                M2L_CreateBattleRoom response = await root.GetComponent<MessageSender>().Call(startSceneConfig.ActorId,
                    request) as M2L_CreateBattleRoom;
                
                L2G_NotifyStartBattle notifyStartBattle = L2G_NotifyStartBattle.Create();
                notifyStartBattle.ActorId = response.ActorId;
                foreach (long id in room.Players)
                {
                    MessageLocationSenderComponent messageLocationSenderComponent = root.GetComponent<MessageLocationSenderComponent>();
                    messageLocationSenderComponent.Get(LocationType.Player).Send(id, notifyStartBattle);
                }
                // Remove Lobby Room
                root.GetComponent<SFSRoomsComponent>().RemoveChild(message.RoomId);
            }
        }
    }
}
