using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSAnimatorComponent))]
    [FriendOf(typeof(SFSAnimatorComponent))]
    [FriendOf(typeof(SFSUnitView))]
    [FriendOf(typeof(SkillComponent))]
    [FriendOf(typeof(SFSUnit))]
    public static partial class SFSAnimatorComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSAnimatorComponent self, SFSUnitView view)
        {
            self.Animator = view.GameObject.GetComponent<Animator>();
            self.Unit = view.Unit;
            self.UnitView = view;
            self.SkillState = ClientSkillState.Idle;
            self.BaseState = ClientBaseState.Idle;
        }

        [EntitySystem]
        private static void Update(this SFSAnimatorComponent self)
        {
            self.UpdateSkillState();
            self.UpdateUnitState();
        }

        private static void UpdateSkillState(this SFSAnimatorComponent self)
        {
            var skillCmpt = self.Unit.GetComponent<SkillComponent>();
            ClientSkillState newState = skillCmpt.State == SFSSkillState.Forward 
                    && skillCmpt.Duration > 0
                    ? ClientSkillState.Skill : ClientSkillState.Idle;
            if (newState != self.SkillState)
            {
                self.SkillState = newState;
                switch (self.SkillState)
                {
                    case ClientSkillState.Idle:
                        self.Animator.SetTrigger(AnimatorHashValue.Empty);
                        break;
                    case ClientSkillState.Skill:
                        self.Animator.SetTrigger(AnimatorHashValue.Skill);
                        break;
                    default:
                        Log.Error($"Not Found SkillState {self.SkillState.ToString()}");
                        break;
                }
            }
        }

        private static void UpdateUnitState(this SFSAnimatorComponent self)
        {
            ClientBaseState newState;
            if (self.Unit.SfsUnitState == SFSUnitState.Die)
                newState = ClientBaseState.Die;
            else if (self.Unit.SfsUnitState == SFSUnitState.Free
                     && !self.Unit.Speed.MyEquals(float3.zero))
                newState = ClientBaseState.Run;
            else
                newState = ClientBaseState.Idle;
            if (self.BaseState != newState)
            {
                self.BaseState = newState;
                switch (self.BaseState)
                {
                    case ClientBaseState.Run:
                        self.Animator.SetTrigger(AnimatorHashValue.Run);
                        break;
                    case ClientBaseState.Die:
                        self.Animator.SetTrigger(AnimatorHashValue.Die);
                        break;
                    case ClientBaseState.Idle:
                        self.Animator.SetTrigger(AnimatorHashValue.Idle);
                        break;
                    default:
                        Log.Error($"Not Found BaseState {self.BaseState.ToString()}");
                        break;
                }
            }
        }

    }
}
