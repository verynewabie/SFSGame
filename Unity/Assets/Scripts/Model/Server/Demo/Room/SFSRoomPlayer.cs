namespace ET.Server
{

    [ChildOf(typeof(SFSRoomPlayerComponent))]
    public class SFSRoomPlayer : Entity, IAwake
    {
        public bool IsOnline = true;
        public bool IsReady = false;
        public int ReconnectStartFrame = 0;
    }
}
