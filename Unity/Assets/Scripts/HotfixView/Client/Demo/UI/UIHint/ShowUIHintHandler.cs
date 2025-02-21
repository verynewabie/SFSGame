namespace ET.Client
{
    
    [Event(SceneType.Demo)]
    public class ShowUIHintHandler : AEvent<Scene, ShowUIHint>
    {
        protected override async ETTask Run(Scene scene, ShowUIHint arg)
        {
            var ui = scene.GetComponent<UIComponent>().Get(UIType.UIHint) ?? 
                    await UIHelper.Create(scene, UIType.UIHint, UILayer.High);
            var cmpt = ui.GetComponent<UIHintComponent>();
            cmpt.ShowWithText(arg.hint, arg.showCloseBtn);

        }
    }
}
