using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
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
        public SFSUnitCamp SfsUnitCamp;
        public float3 Speed;
        public Dictionary<int, MoveCmd> HistoryMoveState = new Dictionary<int, MoveCmd>();
        
        public SFSUnitState SfsUnitState;
        public SFSUnitType SfsUnitType;

        public bool CanReleaseSkill => this.SfsUnitState == SFSUnitState.Free;
        
        public int HP = 100;
    }
}