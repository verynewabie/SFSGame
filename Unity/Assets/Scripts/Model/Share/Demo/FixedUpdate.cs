namespace ET
{


    public class FixedUpdate : Object
    {
        private long startTime;
        private int startFrame;
        private int Interval { get;  set; }

        public FixedUpdate(long startTime, int startFrame, int interval)
        {
            this.startTime = startTime;
            this.startFrame = startFrame;
            this.Interval = interval;
        }
        
        public void ChangeInterval(int interval, int frame)
        {
            this.startTime += (frame - this.startFrame) * this.Interval;
            this.startFrame = frame;
            this.Interval = interval;
        }

        public long FrameTime(int frame)
        {
            return this.startTime + (frame - this.startFrame) * this.Interval;
        }
        
        public void Reset(long time, int frame)
        {
            this.startTime = time;
            this.startFrame = frame;
        }

        public int GetFrame(long serverNow)
        {
            return (int)((serverNow - this.startTime) / this.Interval + this.startFrame);
        }
    }
}
