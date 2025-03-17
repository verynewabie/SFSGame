using Unity.Mathematics;

namespace ET.Client
{
    [FriendOf(typeof(SFSUnit))]
    public static class SFSUnitFactory
    {
        public static SFSUnit Create(BattleRoom room, SFSUnitInfo info)
        {
            SFSUnit result = room.GetComponent<SFSUnitComponent>().AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            result.Position = new float3(0f, 0.5f, 0f);
            result.Rotation = quaternion.identity;
            result.UnitCamp = info.Camp;
            return result;
        }
    }
}
