namespace ET.Client
{
    [EntitySystemOf(typeof(SFSRoom))]
    [FriendOf(typeof(SFSRoom))]
    public static partial class SFSRoomSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.SFSRoom self)
        {
            self.MaxPlayerCount = ConstValue.RoomMaxPlayerCount;
        }

        public static void Show(this ET.Client.SFSRoom self, string holderName, int nowNum)
        {
            
        }
    }
}