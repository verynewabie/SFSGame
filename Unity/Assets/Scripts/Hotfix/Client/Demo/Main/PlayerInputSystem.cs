namespace ET.Client
{

    [EntitySystemOf(typeof(PlayerInput))]
    [FriendOf(typeof(PlayerInput))]
    public static partial class PlayerInputSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.PlayerInput self)
        {
            
        }

        public static void Clear(this ET.Client.PlayerInput self)
        {
            self.A_Down = false;
            self.A_Up = false;
            self.A_Press = false;
            
            self.D_Down = false;
            self.D_Up = false;
            self.D_Press = false;
            
            self.W_Down = false;
            self.W_Up = false;
            self.W_Press = false;
            
            self.S_Down = false;
            self.S_Up = false;
            self.S_Press = false;
            
            self.Q_Press = false;
            self.Q_Down = false;
            self.Q_Up = false;
        }
    }
}
