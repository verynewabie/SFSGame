using Unity.Mathematics;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSUnit))]
    [FriendOf(typeof(SFSUnit))]
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

        public static void HandleCmd(this SFSUnit self, MoveCmd moveCmd)
        {
            if (moveCmd.Dir.x != 0 || moveCmd.Dir.y != 0)
            {
                self.Speed = new float3(moveCmd.Dir.x, 0, moveCmd.Dir.y);
                self.Rotation = quaternion.LookRotation(self.Speed, math.up());
            }
            else
            {
                self.Speed = float3.zero;
            }
        }
    }
}
