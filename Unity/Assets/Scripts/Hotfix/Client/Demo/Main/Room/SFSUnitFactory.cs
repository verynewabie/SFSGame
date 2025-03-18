using Unity.Mathematics;

namespace ET.Client
{
    [FriendOf(typeof(SFSUnit))]
    public static class SFSUnitFactory
    {
        public static void CreateHero(BattleRoom room, SFSUnitInfo info, bool isLocalPlayer)
        {
            SFSUnitComponent component = room.GetComponent<SFSUnitComponent>();
            SFSUnit unit = component.AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            unit.Position = info.Position;
            unit.Rotation = quaternion.identity;
            unit.UnitCamp = info.Camp;
            unit.AddComponent<SkillComponent, SFSUnit>(unit);
            if (isLocalPlayer)
                component.MyUnit = unit;
            // Add UnitView, Animator, Camera
            EventSystem.Instance.Publish(room.Root(), new CreateSFSUnit()
            {
                unit = unit,
                IsLocalPlayer = isLocalPlayer
            });
        }
    }
}
