namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class CreateSFSUnit_CreateUnitView : AEvent<Scene, CreateSFSUnit>
    {
        protected override async ETTask Run(Scene scene, CreateSFSUnit arg)
        {
            BattleRoom room = scene.GetComponent<BattleRoom>();
            var unitViewComponent = room.GetComponent<SFSUnitViewComponent>() ??
                    room.AddComponent<SFSUnitViewComponent>();
            
            var unitView = unitViewComponent.AddChildWithId<SFSUnitView, SFSUnit>(arg.unit.Id, arg.unit);

            await unitView.InitAsync();
        }
    }
}
