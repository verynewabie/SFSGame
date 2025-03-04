using Unity.Mathematics;

namespace ET
{
    [ChildOf(typeof(SFSUnitComponent))]
    public class SFSUnit : Entity, IAwake<BattleRoom>
    {
        public float3 Position;
        public quaternion Rotation;
        public EntityRef<BattleRoom> BattleRoom;
    }
}
