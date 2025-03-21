using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;

namespace ET.Server
{

    [ComponentOf(typeof(BattleRoom))]
    public class CollisionListenerComponent : Entity, IAwake
    {
        public IContactListener ContactListenerImplementation;
        
    }
}
