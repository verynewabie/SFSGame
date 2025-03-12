using Unity.Mathematics;

namespace ET.Client
{

    [EntitySystemOf(typeof(PlayerInputComponent))]
    [FriendOf(typeof(PlayerInputComponent))]
    public static partial class PlayerInputComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.PlayerInputComponent self)
        {
            self.PlayerId = self.Root().GetComponent<PlayerComponent>().MyId;
        }

        private static void Clear(this ET.Client.PlayerInputComponent self)
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

            self.E_Down = false;
            self.E_Press = false;
            self.E_Up = false;
        }

        public static void Tick(this PlayerInputComponent self)
        {
            var sfsComponent = self.GetParent<BattleRoom>().GetComponent<SFSComponent>();
            // Move
            float2 dir = float2.zero;
            if (self.A_Press) dir -= self.Right;
            if (self.D_Press) dir += self.Right;
            if (self.W_Press) dir += self.Forward;
            if (self.S_Press) dir -= self.Forward;
            
            if (dir.x != 0 || dir.y != 0)
                dir = math.normalize(dir);

            MoveCmd moveCmd = MoveCmd.Create();
            moveCmd.Dir = dir;
            moveCmd.CmdType = SFSCmdType.MoveCmd;
            moveCmd.UnitId = self.PlayerId;
            sfsComponent.AddCmdToSendQueue(moveCmd);

            self.Clear();
        }
    }
}
