using Cinemachine;
using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSCameraComponent))]
    [FriendOfAttribute(typeof(ET.Client.SFSUnitView))]
    public static partial class SFSCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.SFSCameraComponent self, ET.Client.SFSUnitView unit)
        {
            GameObject camera = GameObject.Find("VirtualCamera");
            CinemachineVirtualCamera vCamera = camera.GetComponent<CinemachineVirtualCamera>();
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            mainCamera.AddComponent<CinemachineBrain>();
            vCamera.Follow = unit.GameObject.transform;
            vCamera.LookAt = unit.GameObject.transform;
        }
    }
}
