using UnityEngine;

namespace ET.Client
{
    public static class AnimatorHashValue
    {
        [StaticField]
        public static int Die = Animator.StringToHash("Die");
        
        [StaticField]
        public static int Idle = Animator.StringToHash("Idle");
        
        [StaticField]
        public static int Run = Animator.StringToHash("Run");
        
        [StaticField]
        public static int Skill = Animator.StringToHash("Skill");
        
        [StaticField]
        public static int Empty = Animator.StringToHash("Empty");
    }
}
