using UnityEngine.SceneManagement;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class SFSLoadSceneHandler : AEvent<Scene, SFSLoadScene>
    {
        protected override async ETTask Run(Scene scene, SFSLoadScene arg)
        {
            var loader = scene.GetComponent<BattleRoom>().AddComponent<ResourcesLoaderComponent>();
            await loader.LoadSceneAsync($"Assets/Bundles/Scenes/{arg.sceneName}.unity", LoadSceneMode.Single);
        }
    }
}
