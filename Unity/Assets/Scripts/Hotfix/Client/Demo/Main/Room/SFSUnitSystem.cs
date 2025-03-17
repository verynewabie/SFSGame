using Unity.Mathematics;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSUnit))]
    [FriendOf(typeof(SFSUnit))]
    [FriendOf(typeof(SFSComponent))]
    public static partial class SFSUnitSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnit self, BattleRoom room)
        {
            self.BattleRoom = room;
        }

        public static void Tick(this SFSUnit self)
        {
            self.Position += self.Speed * SFSConstValue.UpdateInterval / 1000.0f;
        }

        public static void TickEnd(this SFSUnit self)
        {
            MoveCmd cmd = MoveCmd.Create();
            cmd.Pos = self.Position;
            cmd.Speed = self.Speed;
            cmd.Rot = self.Rotation;
            var sfsCmpt = self.BattleRoom.GetComponent<SFSComponent>();
            self.HistoryMoveState.Add(sfsCmpt.CurrentFrame, cmd);
        }

        public static void HandleCmd(this SFSUnit self, MoveCmd moveCmd)
        {
            self.Position = moveCmd.Pos;
            self.Rotation = moveCmd.Rot;
            self.Speed = moveCmd.Speed;
            if (!moveCmd.Speed.MyEquals(float3.zero))
            {
                self.Rotation = quaternion.LookRotation(self.Speed, math.up());
            }
        }

        public static bool CheckConsistency(this SFSUnit self, int frame, MoveCmd moveCmd)
        {
            if (!self.HistoryMoveState.ContainsKey(frame))
                return false;
            MoveCmd target = self.HistoryMoveState[frame];
            return target.Pos.MyEquals(moveCmd.Pos) &&
                    target.Rot.MyEquals(moveCmd.Rot) &&
                    target.Speed.MyEquals(moveCmd.Speed);
        }

        public static void Rollback(this SFSUnit self, MoveCmd moveCmd)
        {
            self.Position = moveCmd.Pos;
            self.Rotation = moveCmd.Rot;
            self.Speed = moveCmd.Speed;
        }
    }
}
