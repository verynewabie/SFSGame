using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Unity.Mathematics;

namespace ET.Server
{

    [EntitySystemOf(typeof(ColliderComponent))]
    [FriendOf(typeof(ColliderComponent))]
    [FriendOf(typeof(SFSUnit))]
    public static partial class ColliderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ColliderComponent self, CreateColliderInfo info)
        {
            self.Unit = info.unit;
            self.BelongToUnit = info.belongToUnit;
            self.Body = self.Unit.BattleRoom.GetComponent<PhysicsWorldComponent>().World.CreateBody(
                new BodyDef() { BodyType = BodyType.DynamicBody, AllowSleep = false });
            FixtureDef fixtureDef = new FixtureDef();
            fixtureDef.IsSensor = true;
            fixtureDef.Filter = new Filter
            {
                CategoryBits = GetCategoryByUnit(self.Unit),
                MaskBits = GetMaskCategoryByUnit(self.Unit)
            };
            fixtureDef.Shape = new CircleShape
            {
                Position = Vector2.Zero,
                Radius = info.radius
            };
            fixtureDef.UserData = self.Unit;
            self.Body.CreateFixture(fixtureDef);
            self.SyncBody();
        }
        
        public static void Tick(this ColliderComponent self)
        {
            self.SyncBody();
        }
        
        public static void TickEnd(this ColliderComponent self)
        {
        }
        
        private static void SyncBody(this ColliderComponent self)
        {
            self.Body.SetTransform(new Vector2(self.Unit.Position.x, self.Unit.Position.z),self.Body.GetAngle());
        }

        // 1 red hero
        // 2 red pro
        // 4 blue hero
        // 8 blue pro
        private static ushort GetCategoryByUnit(SFSUnit unit)
        {
            if (unit.SfsUnitCamp == SFSUnitCamp.Blue)
            {
                if (unit.SfsUnitType == SFSUnitType.Player)
                    return 4;
                else
                    return 8;
            }
            else
            {
                if (unit.SfsUnitType == SFSUnitType.Player)
                    return 1;
                else
                    return 2;
            }
        }

        private static ushort GetMaskCategoryByUnit(SFSUnit unit)
        {
            if (unit.SfsUnitCamp == SFSUnitCamp.Blue)
            {
                if (unit.SfsUnitType == SFSUnitType.Player)
                    return 2;
                else
                    return 1;
            }
            else
            {
                if (unit.SfsUnitType == SFSUnitType.Player)
                    return 8;
                else
                    return 4;
            }
        }
    }

}
