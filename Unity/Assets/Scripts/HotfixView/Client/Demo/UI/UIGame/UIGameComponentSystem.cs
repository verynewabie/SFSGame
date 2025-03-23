using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [EntitySystemOf(typeof(UIGameComponent))]
    [FriendOf(typeof(SFSComponent))]
    [FriendOf(typeof(UIGameComponent))]
    [FriendOf(typeof(SFSUnit))]
    [FriendOf(typeof(SkillComponent))]
    public static partial class UIGameComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIGameComponent self)
        {
            self.SFSComponent = self.Root().GetComponent<BattleRoom>().GetComponent<SFSComponent>();
            self.MyUnit = self.Root().GetComponent<BattleRoom>().GetComponent<SFSUnitComponent>().MyUnit;
            self.MySkill = self.MyUnit.GetComponent<SkillComponent>();

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.FPS = rc.Get<GameObject>("FPS").GetComponent<Text>();
            self.MS = rc.Get<GameObject>("MS").GetComponent<Text>();
            self.DebugPanel = rc.Get<GameObject>("DebugPanel");
            self.ClientFrame = rc.Get<GameObject>("ClientFrame").GetComponent<Text>();
            self.ServerFrame = rc.Get<GameObject>("ServerFrame").GetComponent<Text>();
            self.FailCount = rc.Get<GameObject>("FailCount").GetComponent<Text>();
            self.ClientAheadFrame = rc.Get<GameObject>("ClientAheadFrame").GetComponent<Text>();
            self.DebugPanelActive = false;

            self.HP = rc.Get<GameObject>("HP").GetComponent<Image>();
            self.Mask = rc.Get<GameObject>("Mask");
            self.CDNum = rc.Get<GameObject>("CDNum").GetComponent<Text>();
        }

        [EntitySystem]
        private static void Update(this UIGameComponent self)
        {
            self.MS.text = $"{(self.SFSComponent.HalfRTT * 2).ToString()}MS";
            self.AccTime += Time.deltaTime;
            self.Num++;
            if (self.AccTime >= 1f)
            {
                self.AccTime -= 1f;
                self.FPS.text = $"{self.Num.ToString()}FPS";
                self.Num = 0;
            }

            // Debug Info
            self.ClientFrame.text = $"{self.SFSComponent.CurrentFrame.ToString()}";
            self.ClientAheadFrame.text = $"{self.SFSComponent.CurrentAheadOfFrame.ToString()}";
            self.ServerFrame.text = $"{(self.SFSComponent.CurrentFrame - self.SFSComponent.CurrentAheadOfFrame).ToString()}";
            self.FailCount.text = $"{self.SFSComponent.FailCount.ToString()}";
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                self.SwitchDebugPanel();
            }

            // HP And CD
            self.HP.fillAmount = self.MyUnit.HP / 100.0f;
            if (self.MySkill.State == SFSSkillState.CD)
            {
                self.Mask.SetActive(true);
                self.CDNum.text = $"{self.MySkill.Duration * 1f / SFSConstValue.FrameCountPerSecond:F2}";
            }
            else 
                self.Mask.SetActive(false);
        }

        private static void SwitchDebugPanel(this UIGameComponent self)
        {
            self.DebugPanelActive = !self.DebugPanelActive;
            self.DebugPanel.SetActive(self.DebugPanelActive);
        }
    }
}
