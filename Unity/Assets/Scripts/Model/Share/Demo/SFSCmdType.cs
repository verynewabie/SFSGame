using MemoryPack;

namespace ET
{
    public enum SFSCmdType
    {
        MoveCmd
    }

    public interface IRoomCmd : ISFSRoomMessage
    {
        int FrameId { get; set; }
        SFSCmdType CmdType { get; set; }
    }
    
    public interface ISFSRoomMessage : IMessage
    {
        long PlayerId { get; set; }
    }
}
