namespace ET.Client
{
    public class StunBuff : SFSBuff
    {
        public StunBuff()
        {
            this.Duration = 2 * SFSConstValue.FrameCountPerSecond;
        }
    }
}
