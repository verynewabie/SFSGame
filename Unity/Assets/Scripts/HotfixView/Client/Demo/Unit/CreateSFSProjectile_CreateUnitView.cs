namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class CreateSFSProjectile_CreateUnitView : AEvent<Scene, CreateSFSProjectile>
    {
        protected override async ETTask Run(Scene scene, CreateSFSProjectile arg)
        {
            BattleRoom room = scene.GetComponent<BattleRoom>();
            var unitViewComponent = room.GetComponent<SFSUnitViewComponent>() ??
                    room.AddComponent<SFSUnitViewComponent>();
            
            var unitView = unitViewComponent.AddChildWithId<SFSUnitView, SFSUnit>(arg.unit.Id, arg.unit);
            await unitView.InitProjectile();
        }
    }
}
