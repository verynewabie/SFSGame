using UnityEngine;

namespace ET.Client
{

    [ComponentOf(typeof(SFSUnitView))]
    public class SFSAnimatorComponent : Entity, IAwake<SFSUnitView>
    {
        public Animator Animator;
    }
}
