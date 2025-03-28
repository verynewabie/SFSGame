using UnityEngine.SceneManagement;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class GameEnd_ChangeScene : AEvent<Scene, GameEnd>
    {
        protected override async ETTask Run(Scene scene, GameEnd arg)
        {
            var loader = scene.GetComponent<BattleRoom>().GetComponent<ResourcesLoaderComponent>();
            await loader.LoadSceneAsync($"Assets/Bundles/Scenes/Empty.unity", LoadSceneMode.Additive);
        }
    }
}
