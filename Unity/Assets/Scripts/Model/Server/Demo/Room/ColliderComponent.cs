using Box2DSharp.Dynamics;

namespace ET.Server
{

    [ComponentOf(typeof(SFSUnit))]
    public class ColliderComponent : Entity, IAwake<CreateColliderInfo>
    {
        public Body Body;
        private EntityRef<SFSUnit> unit;
        public SFSUnit Unit
        {
            get => unit;
            set => unit = value;
        }
        private EntityRef<SFSUnit> belongToUnit;
        public SFSUnit BelongToUnit
        {
            get => belongToUnit;
            set => belongToUnit = value;
        }
    }
    
    public struct CreateColliderInfo
    {
        public SFSUnit unit;
        /// <summary>
        /// 比如A玩家的投掷物。belongTo A玩家
        /// </summary>
        public SFSUnit belongToUnit;
        public float radius;
    }
}
