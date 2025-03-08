using UnityEngine;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class InitBattleViewHandler : AEvent<Scene, InitBattleView>
    {
        protected override async ETTask Run(Scene scene, InitBattleView arg)
        {
            BattleRoom room = scene.GetComponent<BattleRoom>();
            room.AddComponent<SFSOperaComponent, PlayerInput>(arg.playerInput);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            await ETTask.CompletedTask;
        }
    }
}
