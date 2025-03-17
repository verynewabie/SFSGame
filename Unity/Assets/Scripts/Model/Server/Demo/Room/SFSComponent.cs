using System.Collections.Generic;

namespace ET.Server
{

    [ComponentOf(typeof(BattleRoom))]
    public class SFSComponent : Entity, IAwake, IUpdate
    {
        public bool StartSync;
        public FixedUpdate FixedUpdate;
        public int CurrentFrame;
        private EntityRef<BattleRoom> myRoom;
        public BattleRoom MyRoom
        {
            get { return myRoom; }
            set { myRoom = value; }
        }
        /// <summary>
        /// 将要处理的命令列表
        /// </summary>
        public Dictionary<int, Queue<IRoomCmd>> FrameCmdToHandle = new Dictionary<int, Queue<IRoomCmd>>();

        /// <summary>
        /// 将要发送的命令列表
        /// </summary>
        public Dictionary<int, Queue<IRoomCmd>> FrameCmdToSend = new Dictionary<int, Queue<IRoomCmd>>(64);
        
    }
}
