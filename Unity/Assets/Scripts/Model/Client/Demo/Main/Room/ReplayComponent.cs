using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BattleRoom))]
    public class ReplayComponent : Entity, IAwake, IUpdate
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

        public int LastFrame;
        /// <summary>
        /// 将要处理的命令列表
        /// </summary>
        public SortedDictionary<int, Queue<IRoomCmd>> FrameCmdToHandle = new SortedDictionary<int, Queue<IRoomCmd>>();
    }
}
