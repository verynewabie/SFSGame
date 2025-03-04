using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(SFSUnitViewComponent))]
    public class SFSUnitView : Entity, IAwake<SFSUnit>
    {
        public GameObject GameObject;
        public Transform Transform;
        private EntityRef<SFSUnit> unit;
        public SFSUnit Unit
        {
            get => unit;
            set => unit = value;
        }
        private EntityRef<BattleRoom> room;
        public BattleRoom Room
        {
            get => room;
            set => room = value;
        }
    }
}
