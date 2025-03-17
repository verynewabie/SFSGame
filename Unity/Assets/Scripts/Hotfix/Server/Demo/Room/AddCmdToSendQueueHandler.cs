namespace ET.Server
{

    [Event(SceneType.SFSRoom)]
    public class AddCmdToSendQueueHandler : AEvent<Scene, AddCmdToSendQueue>
    {
        protected override async ETTask Run(Scene scene, AddCmdToSendQueue arg)
        {
            scene.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToSendQueue(arg.Cmd);
            await ETTask.CompletedTask;
        }
    }
}
