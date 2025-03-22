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
            // 这些必须等GameObject生成出来再做
            unitView.AddComponent<SFSAnimatorComponent, SFSUnitView>(unitView);
            if (arg.IsLocalPlayer)
            {
                // Add CameraComponent
                room.AddComponent<SFSCameraComponent, SFSUnitView>(unitView);
            }
        }
    }
}
