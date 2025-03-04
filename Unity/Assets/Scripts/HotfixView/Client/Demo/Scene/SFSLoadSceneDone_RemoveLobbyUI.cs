using UnityEngine.SceneManagement;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class SFSLoadSceneDone_RemoveLobbyUI : AEvent<Scene, SFSLoadSceneDone>
    {
        protected override async ETTask Run(Scene scene, SFSLoadSceneDone arg)
        {
            await UIHelper.Remove(scene, UIType.UILobby);
        }
    }
}