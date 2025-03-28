namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Room2C_GameEndHandler : MessageHandler<Scene, Room2C_GameEnd>
    {
        protected override async ETTask Run(Scene root, Room2C_GameEnd message)
        {
            await ETTask.CompletedTask;
            // TODO Game End
        }
    }
}