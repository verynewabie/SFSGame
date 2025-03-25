namespace ET
{
    public enum SFSUnitState
    {
        Free = 0,
        // 异常
        Abnormal = 1, 
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

    public enum SFSBuffType
    {
        Stun,
    }
}
