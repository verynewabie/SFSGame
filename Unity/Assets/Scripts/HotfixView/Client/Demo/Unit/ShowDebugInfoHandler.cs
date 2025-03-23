using UnityEngine;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class ShowDebugInfoHandler : AEvent<Scene, ShowDebugInfo>
    {
        protected override async ETTask Run(Scene scene, ShowDebugInfo arg)
        {
            scene.GetComponent<BattleRoom>().GetComponent<GraphicsDebugComponent>()?.Render(arg);
            await ETTask.CompletedTask;
        }
    }
}