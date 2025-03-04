namespace ET
{

    [EntitySystemOf(typeof(SFSUnit))]
    [FriendOfAttribute(typeof(ET.SFSUnit))]
    public static partial class SFSUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ET.SFSUnit self, ET.BattleRoom room)
        {
            self.BattleRoom = room;
        }
    }
}
