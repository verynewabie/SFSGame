using Unity.Mathematics;

namespace ET
{
    [FriendOfAttribute(typeof(ET.SFSUnit))]
    public static partial class SFSUnitFactory
    {
        public static SFSUnit Create(BattleRoom room, SFSUnitInfo info)
        {
            SFSUnit result = room.GetComponent<SFSUnitComponent>().AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            if (info.Camp == UnitCamp.Home)
            {
                result.Position = new float3(5, 0, 0);
                result.Rotation = quaternion.identity;
            }
            else
            {
                result.Position = new float3(-5, 0, 0);
                result.Rotation = quaternion.LookRotation(new float3(0, 0, -1), new float3(0, 1, 0));
            }
            return result;
        }
    }
}
