namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    [FriendOf(typeof(SFSUnit))]
    public class Room2C_GameEndHandler : MessageHandler<Scene, Room2C_GameEnd>
    {
        protected override async ETTask Run(Scene root, Room2C_GameEnd message)
        {
            SFSUnit myUnit = root.GetComponent<BattleRoom>().GetComponent<SFSUnitComponent>().MyUnit;
            bool win = myUnit.SfsUnitCamp == message.WinCamp;
            await EventSystem.Instance.PublishAsync(root, new ShowUIHint
            {
                hint = win ? $"Win!" : "Lose",
                showCloseBtn = true
            });
            await EventSystem.Instance.PublishAsync(root, new GameEnd());
            root.RemoveComponent<BattleRoom>();
        }
    }
}