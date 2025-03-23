using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [ComponentOf(typeof(UI))]
    public class UIGameComponent : Entity, IAwake, IUpdate
    {
        private EntityRef<SFSComponent> sfsComponent;
        public SFSComponent SFSComponent
        {
            get { return sfsComponent; }
            set { sfsComponent = value; }
        }
        private EntityRef<SFSUnit> myUnit;
        public SFSUnit MyUnit
        {
            get { return myUnit; }
            set { myUnit = value; }
        }
        private EntityRef<SkillComponent> mySkill;
        public SkillComponent MySkill
        {
            get { return mySkill; }
            set { mySkill = value; }
        }

        public float AccTime;
        public int Num;
        public Text FPS;
        public Text MS;
        public GameObject DebugPanel;
        public bool DebugPanelActive;
        public Text ClientFrame;
        public Text ServerFrame;
        public Text ClientAheadFrame;
        public Text FailCount;

        public Image HP;
        public GameObject Mask;
        public Text CDNum;
    }
}
