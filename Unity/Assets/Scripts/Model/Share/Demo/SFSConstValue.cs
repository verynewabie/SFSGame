namespace ET
{
    public static class SFSConstValue
    {
        public const int UpdateInterval = 25;
        public const int FrameCountPerSecond = 1000 / UpdateInterval;
        public const bool EnableDelayCompensation = false;
        public const bool EnableSmoothUpdateInterval = false;
        public const int SkillCDFrame = 5 * FrameCountPerSecond;
        public const float UpdateIntervalFloat = UpdateInterval / 1000.0f;
        public const int SkillForward = FrameCountPerSecond / 10 * 4;
        public const int SkillCD = 5 * FrameCountPerSecond;
    }
}
