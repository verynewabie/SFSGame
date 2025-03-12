using System;

namespace ET
{


    public static class TimeAndFrameConverter
    {
        public static int Long2Frame(long time)
        {
            return (int)Math.Ceiling(time * 1.0 / SFSConstValue.UpdateInterval);
        }
    }
}
