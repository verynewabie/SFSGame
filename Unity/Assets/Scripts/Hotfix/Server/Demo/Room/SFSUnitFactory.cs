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
            unit.Rotation = quaternion.identity;
            unit.UnitCamp = info.Camp;
            unit.AddComponent<SkillComponent, SFSUnit>(unit);
            return unit;
        }
    }
}