using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSAnimatorComponent))]
    [FriendOf(typeof(SFSAnimatorComponent))]
    [FriendOf(typeof(SFSUnitView))]
    public static partial class SFSAnimatorComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSAnimatorComponent self, SFSUnitView view)
        {
            self.Animator = view.GameObject.GetComponent<Animator>();
        }
    }
}
