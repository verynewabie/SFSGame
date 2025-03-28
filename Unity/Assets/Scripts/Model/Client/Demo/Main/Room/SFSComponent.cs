using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 管理Tick的Component
    /// </summary>
    [ComponentOf(typeof(BattleRoom))]
    public class SFSComponent : Entity, IAwake, IUpdate
    {
        public long LocalPlayerId;
        public bool StartSync;
        public FixedUpdate ClientUpdate;
        public FixedUpdate ServerUpdate;
        public int FrameBuffer = 1;
        public int CurrentFrame;
        public int CurrentArrivedFrame;
        public bool HasInSpeedChangeState;
        public int FailCount;
        private EntityRef<BattleRoom> myRoom;
        public BattleRoom MyRoom
        {
            get { return myRoom; }
            set { myRoom = value; }
        }
        /// <summary>
        /// 将要处理的命令列表
        /// </summary>
        public SortedDictionary<int, Queue<IRoomCmd>> FrameCmdToHandle = new SortedDictionary<int, Queue<IRoomCmd>>();
        
        /// <summary>
        /// 将要发送的命令列表
        /// </summary>
        public Dictionary<int, Queue<IRoomCmd>> FrameCmdToSend = new Dictionary<int, Queue<IRoomCmd>>(64);
        
        /// <summary>
        /// 玩家输入缓冲区，因为会有回滚操作，需要重新预测到当前帧，保存范围为上一次服务器确认的帧到当前帧
        /// </summary>
        public Dictionary<int, Queue<IRoomCmd>> PlayerInputCmdBuffer = new Dictionary<int, Queue<IRoomCmd>>();
        
        /// <summary>
        /// 当前客户端超前服务端的帧数
        /// </summary>
        public int CurrentAheadOfFrame;

        /// <summary>
        /// 客户端应当超前服务端的帧数
        /// </summary>
        public int TargetAheadOfFrame;
        
        /// <summary>
        /// 从客户端到服务端通信所要花费的时间（ms）半个RTT（不包括服务端的缓存帧时长）
        /// </summary>
        public long HalfRTT;

        /// <summary>
        /// 预测回滚后追帧时标记一下，此时只需要Tick MyUnit
        /// </summary>
        public bool IsInChaseFrameState;
    }
}
