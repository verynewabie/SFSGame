using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(SFSUnit))]
    public static class SFSUnitFactory
    {
        public static SFSUnit CreateHero(BattleRoom room, SFSUnitInfo info)
        {
            SFSUnit unit = room.GetComponent<SFSUnitComponent>().AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            unit.Position = info.Position;
            unit.Rotation = info.Forward;
            unit.SfsUnitType = info.Type;
            unit.SfsUnitCamp = info.Camp;
            unit.SfsUnitState = info.State;
            unit.AddComponent<SkillComponent, SFSUnit>(unit);
            CreateColliderInfo colliderInfo = new CreateColliderInfo
            {  
                radius = 1f,
                unit = unit,
            };
            unit.AddComponent<ColliderComponent, CreateColliderInfo>(colliderInfo);
            unit.AddComponent<BuffComponent, SFSUnit>(unit);
            return unit;
        }

        public static void CreateProjectile(BattleRoom room, SFSUnitInfo info)
        {
            SFSUnit unit = room.GetComponent<SFSUnitComponent>().AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            unit.Position = info.Position;
            unit.Rotation = info.Forward;
            unit.SfsUnitType = info.Type;
            unit.SfsUnitCamp = info.Camp;
            unit.SfsUnitState = info.State;
            unit.Speed = math.forward(unit.Rotation) * 2.5f;
            CreateColliderInfo colliderInfo = new CreateColliderInfo
            {  
                radius = 0.25f,
                unit = unit,
            };
            unit.AddComponent<ColliderComponent, CreateColliderInfo>(colliderInfo);
        }
    }
}