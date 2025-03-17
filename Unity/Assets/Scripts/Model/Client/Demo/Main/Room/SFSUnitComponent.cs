namespace ET.Client
{
    [ComponentOf(typeof(BattleRoom))]
    public class SFSUnitComponent :Entity, IAwake
    {
        private EntityRef<SFSUnit> myUnit;
        public EntityRef<SFSUnit> MyUnit
        {
            get => myUnit;
            set => myUnit = value;
        }
    }
}
