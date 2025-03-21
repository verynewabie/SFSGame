using System.Collections.Generic;
using Box2DSharp.Dynamics;

namespace ET.Server
{

    [ComponentOf(typeof(BattleRoom))]
    public class PhysicsWorldComponent : Entity, IAwake, IDestroy
    {
        private Box2DSharp.Dynamics.World world;
        public Box2DSharp.Dynamics.World World
        {
            get => this.world;
            set => this.world = value;
        }
        
        public List<Body> BodyToDestroy = new List<Body>();
        
        public const int VelocityIteration = 10;
        public const int PositionIteration = 10;
    }
}
