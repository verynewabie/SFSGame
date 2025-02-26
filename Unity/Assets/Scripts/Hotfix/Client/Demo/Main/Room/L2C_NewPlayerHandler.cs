namespace ET.Client
{

    [MessageHandler(SceneType.Demo)]
    public class L2C_NewPlayerHandler : MessageHandler<Scene, L2C_NewPlayer>
    {
        protected override async ETTask Run(Scene root, L2C_NewPlayer message)
        {
            await ETTask.CompletedTask;
            EventSystem.Instance.PublishAsync(root, new PlayerEnterRoom
            {
                name = message.Name,
                playerId = message.PlayerId
            }).Coroutine();
        }
    }
}
