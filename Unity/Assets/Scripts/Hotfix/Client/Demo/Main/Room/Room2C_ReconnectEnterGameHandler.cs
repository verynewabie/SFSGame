namespace ET.Client
{

    [MessageHandler(SceneType.Demo)]
    public class Room2C_ReconnectEnterGameHandler : MessageHandler<Scene, Room2C_ReconnectEnterGame>
    {
        protected override async ETTask Run(Scene root, Room2C_ReconnectEnterGame message)
        {
            await EventSystem.Instance.PublishAsync(root, new HideUIHint());
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().StartSync(message.StartTime);
        }
    }
}
