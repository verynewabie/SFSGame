using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(SFSUnitView))]
    public class SFSAnimatorComponent : Entity, IAwake<SFSUnitView>, IUpdate
    {
        public Animator Animator;
        private EntityRef<SFSUnit> unit;
        public SFSUnit Unit
        {
            get => this.unit;
            set => this.unit = value;
        }
        private EntityRef<SFSUnitView> unitView;
        public SFSUnitView UnitView
        {
            get => this.unitView;
            set => this.unitView = value;
        }

        public ClientSkillState SkillState;
        public ClientBaseState BaseState;
    }

    public enum ClientSkillState
    {
        Idle,
        Skill
    }

    public enum ClientBaseState
    {
        Idle,
        Run,
        Die
    }
}
