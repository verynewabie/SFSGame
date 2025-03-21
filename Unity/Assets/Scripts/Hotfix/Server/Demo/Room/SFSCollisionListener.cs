using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;

namespace ET.Server
{
    [EnableClass]
    public class SFSCollisionListener : IContactListener
    {
        // 这些方法在PhysicsWorld Tick时触发
        public void BeginContact(Contact contact)
        {
            SFSUnit unitA = (SFSUnit)contact.FixtureA.UserData;
            SFSUnit unitB = (SFSUnit)contact.FixtureB.UserData;
            if (unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }

            Log.Error($"UnitA Contact UnitB");
        }

        public void EndContact(Contact contact)
        {
            
        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {
            
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {
            
        }
    }
}
