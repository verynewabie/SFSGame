using UnityEngine.SceneManagement;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class RemoveReplayUIHander : AEvent<Scene, RemoveReplayUI>
    {
        protected override async ETTask Run(Scene scene, RemoveReplayUI arg)
        {
            await UIHelper.Remove(scene, UIType.UIReplay);
        }
    }
}