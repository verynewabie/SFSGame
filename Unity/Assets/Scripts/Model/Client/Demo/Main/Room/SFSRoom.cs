namespace ET.Client
{
    // TODO 在大厅中同步需要维护SFSRoom相关的数据结构
    [ChildOf(typeof(SFSRoomsComponent))]
    public class SFSRoom : Entity, IAwake
    {
        public string RoomHolderName;
        public int PlayerCount;
        public int MaxPlayerCount;
    }
}
