using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;

namespace ET.Server
{
    [EnableClass]
    public class SFSCollisionListener : IContactListener
    {
        // 这些方法在PhysicsWorld Tick/World Step时触发
        public void BeginContact(Contact contact)
        {
            SFSUnit unitA = (SFSUnit)contact.FixtureA.UserData; // Player
            SFSUnit unitB = (SFSUnit)contact.FixtureB.UserData; // Projectile
            if (unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }
            
            // Destroy Unit, Remove Body
            EventSystem.Instance.Publish(unitB.Root(), new AddUnitToRemove
            {
                UnitId = unitB.Id
            });
            EventSystem.Instance.Publish(unitA.Root(), new AddBodyToRemove
            {
                Body = contact.FixtureB.Body
            });
            // Add Buff, Take Damage
            
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
