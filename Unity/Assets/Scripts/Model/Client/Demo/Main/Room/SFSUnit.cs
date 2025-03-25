using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [ChildOf(typeof(SFSUnitComponent))]
    public class SFSUnit : Entity, IAwake<BattleRoom>
    {
        public SFSUnitCamp SfsUnitCamp;
        public SFSUnitType SfsUnitType;
        private EntityRef<BattleRoom> battleRoom;
        public BattleRoom BattleRoom
        {
            get { return battleRoom; }
            set { battleRoom = value; }
        }
        public bool CanReleaseSkill => this.SfsUnitState == SFSUnitState.Free;
        
        public float3 Position;
        public quaternion Rotation;
        public float3 Speed;
        public Dictionary<int, MoveCmd> HistoryMoveState = new Dictionary<int, MoveCmd>();
        
        public SFSUnitState SfsUnitState;
        public int Duration;
        public Dictionary<int, StateCmd> HistoryState = new Dictionary<int, StateCmd>();
        
        public int HP = 100;
        public Dictionary<int,AttributeCmd> HistoryAttribute = new Dictionary<int,AttributeCmd>();
    }
}
