using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Server
{

    [EntitySystemOf(typeof(PhysicsWorldComponent))]
    [FriendOf(typeof(PhysicsWorldComponent))]
    public static partial class PhysicsWorldComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PhysicsWorldComponent self)
        {
            self.World = new Box2DSharp.Dynamics.World(new Vector2(0, 0));
        }
        
        [EntitySystem]
        private static void Destroy(this PhysicsWorldComponent self)
        {
            foreach (var body in self.BodyToDestroy)
            {
                self.World.DestroyBody(body);
            }
            self.BodyToDestroy.Clear();
            
            self.World.Dispose();
            self.World = null;
        }

        public static void Tick(this PhysicsWorldComponent self)
        {
            foreach (Body body in self.BodyToDestroy)
            {
                self.World.DestroyBody(body);
            }
            self.BodyToDestroy.Clear();
            self.World.Step(SFSConstValue.UpdateIntervalFloat, PhysicsWorldComponent.VelocityIteration,
                PhysicsWorldComponent.PositionIteration);
        }

        public static void AddBodyTobeDestroyed(this PhysicsWorldComponent self, Body body)
        {
            self.BodyToDestroy.Add(body);
        }
    }
}
