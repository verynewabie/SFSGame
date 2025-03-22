using MemoryPack;

namespace ET
{
    public enum SFSCmdType
    {
        MoveCmd,
        SkillCmd,
        DeleteUnitCmd,
    }

    public interface IRoomCmd : IMessage
    {
        int FrameId { get; set; }
        SFSCmdType CmdType { get; set; }
        long UnitId { get; set; }
        bool PassConsistencyCheck { get; set; }
    }
    
    public interface ISFSRoomMessage : IMessage
    {
        long PlayerId { get; set; }
    }
}
