namespace ET.Server
{

    [Event(SceneType.SFSRoom)]
    public class AddUnitCreateInfoHandler : AEvent<Scene, AddUnitCreateInfo>
    {
        protected override async ETTask Run(Scene scene, AddUnitCreateInfo arg)
        {
            scene.GetComponent<BattleRoom>().GetComponent<SFSUnitComponent>().AddUnitToCreate(arg.Info);
            await ETTask.CompletedTask;
        }
    }
}