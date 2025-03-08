using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSOperaComponent))]
    [FriendOf(typeof(PlayerInput))]
    [FriendOfAttribute(typeof(ET.Client.SFSOperaComponent))]
    public static partial class SFSOperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.SFSOperaComponent self, PlayerInput input)
        {
            self.PlayerInput = input;
        }
        [EntitySystem]
        private static void Update(this ET.Client.SFSOperaComponent self)
        {
            PlayerInput input = self.PlayerInput;
            
            input.A_Down = Input.GetKeyDown(KeyCode.A);
            input.A_Up = Input.GetKeyUp(KeyCode.A);
            input.A_Press = Input.GetKey(KeyCode.A);
            
            input.D_Down = Input.GetKeyDown(KeyCode.D);
            input.D_Up = Input.GetKeyUp(KeyCode.D);
            input.D_Press = Input.GetKey(KeyCode.D);
            
            input.W_Down = Input.GetKeyDown(KeyCode.W);
            input.W_Up = Input.GetKeyUp(KeyCode.W);
            input.W_Press = Input.GetKey(KeyCode.W);
            
            input.S_Down = Input.GetKeyDown(KeyCode.S);
            input.S_Up = Input.GetKeyUp(KeyCode.S);
            input.S_Press = Input.GetKey(KeyCode.S);
            
            input.Q_Down = Input.GetKeyDown(KeyCode.Q);
            input.Q_Up = Input.GetKeyUp(KeyCode.Q);
            input.Q_Press = Input.GetKey(KeyCode.Q);
        }
    }
}
