namespace ET.Server
{

    [MessageHandler(SceneType.SFSRoom)]
    public class C2Room_CmdHandler : MessageHandler<Scene, MoveCmd>
    {
        protected override async ETTask Run(Scene root, MoveCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
}
