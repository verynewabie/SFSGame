namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class InitBattleViewHandler : AEvent<Scene, InitBattleView>
    {
        protected override async ETTask Run(Scene scene, InitBattleView a)
        {
            await ETTask.CompletedTask;
        }
    }
}
