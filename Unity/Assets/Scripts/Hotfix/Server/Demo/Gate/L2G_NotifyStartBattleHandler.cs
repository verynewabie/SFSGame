namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    public class L2G_NotifyStartBattleHandler : MessageHandler<Player, L2G_NotifyStartBattle>
    {
        protected override async ETTask Run(Player player, L2G_NotifyStartBattle message)
        {
            player.AddComponent<PlayerRoomComponent>().RoomActorId = message.ActorId;
            await ETTask.CompletedTask;
        }
    }
}