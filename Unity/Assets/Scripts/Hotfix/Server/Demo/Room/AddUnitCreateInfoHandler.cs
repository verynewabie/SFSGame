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
    
    [Event(SceneType.SFSRoom)]
    public class AddUnitToRemoveHandler : AEvent<Scene, AddUnitToRemove>
    {
        protected override async ETTask Run(Scene scene, AddUnitToRemove arg)
        {
            scene.GetComponent<BattleRoom>().GetComponent<SFSUnitComponent>().AddUnitToDelete(arg.UnitId);
            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.SFSRoom)]
    public class AddBodyToRemoveHandler : AEvent<Scene, AddBodyToRemove>
    {
        protected override async ETTask Run(Scene scene, AddBodyToRemove arg)
        {
            scene.GetComponent<BattleRoom>().GetComponent<PhysicsWorldComponent>().AddBodyTobeDestroyed(arg.Body);
            await ETTask.CompletedTask;
        }
    }
}