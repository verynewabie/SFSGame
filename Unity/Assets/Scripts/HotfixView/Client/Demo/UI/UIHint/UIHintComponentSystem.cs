using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [EntitySystemOf(typeof(UIHintComponent))]
    [FriendOf(typeof(UIHintComponent))]
    public static partial class UIHintComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.UIHintComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.hintText = rc.Get<GameObject>("HintText").GetComponent<Text>();
			self.closeBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            self.closeBtn.onClick.AddListener(() => self.Hide());
            self.root = rc.Get<GameObject>("Root");
        }

        public static void ShowWithText(this UIHintComponent self, string text, bool showBtn)
        {
            self.root.SetActive(true);
            self.hintText.text = text;
            self.closeBtn.gameObject.SetActive(showBtn);
        }

        public static void Hide(this UIHintComponent self)
        {
            self.root.SetActive(false);
        }
    }
}
