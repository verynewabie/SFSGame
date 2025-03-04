namespace ET
{
    public interface IRoomMessage: IMessage
    {
        long PlayerId { get; set; }
    }

    public interface ISFSRoomMessage : IMessage
    {
        long PlayerId { get; set; }
    }
}