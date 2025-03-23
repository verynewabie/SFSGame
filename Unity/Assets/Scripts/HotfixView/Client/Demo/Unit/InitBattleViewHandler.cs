using UnityEngine;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class InitBattleViewHandler : AEvent<Scene, InitBattleView>
    {
        protected override async ETTask Run(Scene scene, InitBattleView arg)
        {
            BattleRoom room = scene.GetComponent<BattleRoom>();
            room.AddComponent<SFSOperaComponent, PlayerInputComponent>(arg.PlayerInputComponent);
            room.AddComponent<UIComponent>();
            // TODO Remove Debug Info
            // room.AddComponent<GraphicsDebugComponent>();
            await UIHelper.Create(room, UIType.UIGame, UILayer.Mid);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            await ETTask.CompletedTask;
        }
    }
}
