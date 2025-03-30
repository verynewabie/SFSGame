using UnityEngine;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class InitReplayBattleViewHandler : AEvent<Scene, InitReplayBattleView>
    {
        protected override async ETTask Run(Scene scene, InitReplayBattleView arg)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            await ETTask.CompletedTask;
        }
    }
}