using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSOperaComponent))]
    [FriendOf(typeof(PlayerInputComponent))]
    [FriendOf(typeof(SFSOperaComponent))]
    public static partial class SFSOperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSOperaComponent self, PlayerInputComponent inputComponent)
        {
            self.PlayerInput = inputComponent;
        }
        [EntitySystem]
        private static void Update(this SFSOperaComponent self)
        {
            PlayerInputComponent inputComponent = self.PlayerInput;
            
            inputComponent.A_Down |= Input.GetKeyDown(KeyCode.A);
            inputComponent.A_Up |= Input.GetKeyUp(KeyCode.A);
            inputComponent.A_Press |= Input.GetKey(KeyCode.A);
            
            inputComponent.D_Down |= Input.GetKeyDown(KeyCode.D);
            inputComponent.D_Up |= Input.GetKeyUp(KeyCode.D);
            inputComponent.D_Press |= Input.GetKey(KeyCode.D);
            
            inputComponent.W_Down |= Input.GetKeyDown(KeyCode.W);
            inputComponent.W_Up |= Input.GetKeyUp(KeyCode.W);
            inputComponent.W_Press |= Input.GetKey(KeyCode.W);
            
            inputComponent.S_Down |= Input.GetKeyDown(KeyCode.S);
            inputComponent.S_Up |= Input.GetKeyUp(KeyCode.S);
            inputComponent.S_Press |= Input.GetKey(KeyCode.S);
            
            inputComponent.Q_Down |= Input.GetKeyDown(KeyCode.Q);
            inputComponent.Q_Up |= Input.GetKeyUp(KeyCode.Q);
            inputComponent.Q_Press |= Input.GetKey(KeyCode.Q);
            
            inputComponent.E_Down |= Input.GetKeyDown(KeyCode.E);
            inputComponent.E_Up |= Input.GetKeyUp(KeyCode.E);
            inputComponent.E_Press |= Input.GetKey(KeyCode.E);

            Transform cameraTransform = Camera.main.transform;
            inputComponent.Forward = new float2(cameraTransform.forward.x, cameraTransform.forward.z);
            inputComponent.Right = new float2(cameraTransform.right.x, cameraTransform.right.z);
        }
    }
}
