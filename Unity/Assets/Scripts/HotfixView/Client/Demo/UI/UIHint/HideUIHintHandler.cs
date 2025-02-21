namespace ET.Client
{
    
    [Event(SceneType.Demo)]
    public class HideUIHintHandler : AEvent<Scene,HideUIHint>
    {
        protected override async ETTask Run(Scene scene, HideUIHint arg)
        {
            scene.GetComponent<UIComponent>().Get(UIType.UIHint)
                    .GetComponent<UIHintComponent>().Hide();
            await ETTask.CompletedTask;
        }
    }
}
