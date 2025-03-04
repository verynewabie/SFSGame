namespace ET.Client
{
        
    [MessageHandler(SceneType.Demo)]
    public class Room2C_SFSEnterGameHandler : MessageHandler<Scene, Room2C_SFSEnterGame>
    {
        protected override async ETTask Run(Scene root, Room2C_SFSEnterGame message)
        {
            await EventSystem.Instance.PublishAsync(root, new HideUIHint());
        }
    }
}
