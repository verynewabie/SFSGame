using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.UIGame)]
    public class UIGameEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            string assetsName = $"Assets/Bundles/UI/Demo/{UIType.UIGame}.prefab";
            GameObject bundleGameObject = await uiComponent.Root().GetComponent<BattleRoom>().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIGame, gameObject);
            ui.AddComponent<UIGameComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}