using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [EntitySystemOf(typeof(UIGameComponent))]
    [FriendOf(typeof(SFSComponent))]
    [FriendOf(typeof(UIGameComponent))]
    public static partial class UIGameComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIGameComponent self)
        {
            self.SFSComponent = self.Root().GetComponent<BattleRoom>().GetComponent<SFSComponent>();
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            
            self.FPS = rc.Get<GameObject>("FPS").GetComponent<Text>();
            self.MS = rc.Get<GameObject>("MS").GetComponent<Text>();
        }
        [EntitySystem]
        private static void Update(this UIGameComponent self)
        {
            self.MS.text = (self.SFSComponent.HalfRTT * 2).ToString();
            self.AccTime += Time.deltaTime;
            self.Num++;
            if (self.AccTime >= 1f)
            {
                self.AccTime -= 1f;
                self.FPS.text = self.Num.ToString();
                self.Num = 0;
            }
        }
    }
}
