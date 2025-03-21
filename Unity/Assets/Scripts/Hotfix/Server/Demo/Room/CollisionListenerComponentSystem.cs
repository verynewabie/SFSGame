using Box2DSharp.Dynamics.Contacts;

namespace ET.Server
{

    [EntitySystemOf(typeof(CollisionListenerComponent))]
    [FriendOf(typeof(CollisionListenerComponent))]
    public static partial class CollisionListenerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CollisionListenerComponent self)
        {
            self.ContactListenerImplementation = new SFSCollisionListener();
            self.GetParent<BattleRoom>().GetComponent<PhysicsWorldComponent>().World.SetContactListener(self.ContactListenerImplementation);
        }
        
    }
}
