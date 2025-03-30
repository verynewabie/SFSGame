using Cinemachine;
using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSCameraComponent))]
    [FriendOf(typeof(SFSUnitView))]
    public static partial class SFSCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSCameraComponent self, SFSUnitView unit)
        {
            GameObject camera = GameObject.Find("VirtualCamera");
            CinemachineVirtualCamera vCamera = camera.GetComponent<CinemachineVirtualCamera>();
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            mainCamera.AddComponent<CinemachineBrain>();
            vCamera.Follow = unit.GameObject.transform;
            vCamera.LookAt = unit.GameObject.transform;
        }
        
        [EntitySystem]
        private static void Update(this SFSCameraComponent self)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
