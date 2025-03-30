using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class GameEndHandler : AEvent<Scene, GameEnd>
    {
        protected override async ETTask Run(Scene scene, GameEnd arg)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            var loader = scene.GetComponent<BattleRoom>().GetComponent<ResourcesLoaderComponent>();
            await loader.LoadSceneAsync($"Assets/Bundles/Scenes/Empty.unity", LoadSceneMode.Additive);
            await UIHelper.Create(scene, UIType.UIReplay, UILayer.Mid);
        }
    }
}
