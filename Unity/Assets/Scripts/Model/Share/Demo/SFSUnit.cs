using Unity.Mathematics;

namespace ET
{
    [ChildOf(typeof(SFSUnitComponent))]
    public class SFSUnit : Entity, IAwake<BattleRoom>
    {
        public float3 Position;
        public quaternion Rotation;
        private EntityRef<BattleRoom> battleRoom;
        public BattleRoom BattleRoom
        {
            get { return battleRoom; }
            set { battleRoom = value; }
        }
        public UnitCamp UnitCamp;
        public float3 Speed;
    }
}
