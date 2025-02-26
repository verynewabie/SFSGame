using System.Collections.Generic;

namespace ET.Server
{

    [ChildOf(typeof(SFSRoomsComponent))]
    public class SFSRoom:Entity,IAwake<long>
    {
        public long RoomHolderId;
        public List<long> Players;
    }

}
