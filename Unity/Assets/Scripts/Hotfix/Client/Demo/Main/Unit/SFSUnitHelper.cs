namespace ET.Client
{
    public static class SFSUnitHelper
    {
        public static SFSUnit GetMyUnit(Scene root)
        {
            long playerId = root.GetComponent<PlayerComponent>().MyId;
            return root.GetComponent<BattleRoom>()?.GetComponent<SFSUnitComponent>()?.GetChild<SFSUnit>(playerId);
        }
    }
}
