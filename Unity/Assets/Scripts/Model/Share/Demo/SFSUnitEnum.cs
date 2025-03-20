namespace ET
{
    public enum SFSUnitState
    {
        Free = 0,
        Forward = 1, // 施法前摇
        Abnormal = 2, // 异常
    }

    public enum SFSUnitType
    {
        Player = 0,
        Projectile = 1,
    }
    
    public enum SFSUnitCamp
    {
        Red = 0,
        Blue = 1,
    }

    public enum SFSSkillState
    {
        None = 0,
        Forward = 1,
        CD = 2,
    }
}
