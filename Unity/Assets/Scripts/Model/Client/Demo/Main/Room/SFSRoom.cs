namespace ET.Client
{

    [ChildOf(typeof(SFSRoomsComponent))]
    public class SFSRoom : Entity, IAwake
    {
        public string RoomHolderName;
        public int PlayerCount;
        public int MaxPlayerCount;
    }
}
