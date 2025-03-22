namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class RemoveUnitViewHandler : AEvent<Scene, RemoveUnitView>
    {
        protected override async ETTask Run(Scene scene, RemoveUnitView arg)
        {
            await ETTask.CompletedTask;
            var unitViewComponent = scene.GetComponent<BattleRoom>()
                    .GetComponent<SFSUnitViewComponent>();
            foreach (long id in arg.UnitToDelete)
            {
                unitViewComponent.RemoveChild(id);
            }
        }
    }
}