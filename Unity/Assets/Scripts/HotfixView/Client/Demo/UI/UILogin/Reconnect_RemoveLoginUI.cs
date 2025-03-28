namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class Reconnect_RemoveLoginUI : AEvent<Scene, Reconnect>
    {
        protected override async ETTask Run(Scene scene, Reconnect arg)
        {
            await UIHelper.Remove(scene, UIType.UILogin);
        }
    }
}
