namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Room2C_CmdHandler : MessageHandler<Scene, MoveCmd>
    {
        protected override async ETTask Run(Scene root, MoveCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
}
