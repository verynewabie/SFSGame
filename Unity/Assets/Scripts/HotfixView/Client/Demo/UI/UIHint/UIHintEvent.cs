using UnityEngine;

namespace ET.Client
{

    [UIEvent(UIType.UIHint)]
    public class UIHintEvent : AUIEvent

    {
    public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
    {
        string assetsName = $"Assets/Bundles/UI/Demo/{UIType.UIHint}.prefab";
        GameObject bundleGameObject = await uiComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
        GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
        UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIHint, gameObject);
        ui.AddComponent<UIHintComponent>();
        return ui;
    }

    public override void OnRemove(UIComponent uiComponent)
    {
    }
    }
}
