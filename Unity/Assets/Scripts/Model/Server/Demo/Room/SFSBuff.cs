namespace ET.Server
{
    public class SFSBuff : Object
    {
        /// <summary>
        /// 总持续时长，帧
        /// </summary>
        protected int Duration;
        private int RemainFrame;

        public virtual void ApplyEffect(SFSUnit target)
        {
            this.RemainFrame = this.Duration;
        }

        /// <summary>
        /// 更新Buff
        /// </summary>
        /// <returns>Buff是否结束</returns>
        public virtual bool UpdateBuff()
        {
            this.RemainFrame--;
            return this.RemainFrame == 0;
        }

        /// <summary>
        /// 移除 Buff 时清理效果
        /// </summary>
        public virtual void RemoveBuff()
        {
            
        }
    }
}
