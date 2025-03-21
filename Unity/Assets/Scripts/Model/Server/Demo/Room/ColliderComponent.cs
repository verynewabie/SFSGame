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
    }
    
    public struct CreateColliderInfo
    {
        public SFSUnit unit;
        public float radius;
    }
}
