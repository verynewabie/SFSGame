namespace ET.Server
{
    public class StunBuff : SFSBuff
    {
        public StunBuff()
        {
            this.Duration = 50;
        }

        public override void ApplyEffect(SFSUnit target)
        {
            base.ApplyEffect(target);
            target.ChangeUnitState(SFSUnitState.Abnormal, this.Duration);
        }
    }
}
