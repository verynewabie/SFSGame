namespace ET.Client
{

    [EntitySystemOf(typeof(SFSUnitComponent))]
    [FriendOf(typeof(SFSUnit))]
    public static partial class SFSUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnitComponent self)
        {

        }
        
        public static void OnlyTick(this SFSUnitComponent self)
        {
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit && unit.SfsUnitState != SFSUnitState.Die)
                {
                    unit.Tick();
                }
            }
        }

        public static void Tick(this SFSUnitComponent self, bool isInChaseFrameState)
        {
            if (isInChaseFrameState)
            {
                SFSUnit unit = self.MyUnit;
                if (unit.SfsUnitState != SFSUnitState.Die)
                {
                    unit.Tick();
                    unit.TickEnd();
                }
                return;
            }
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit && unit.SfsUnitState != SFSUnitState.Die)
                {
                    unit.Tick();
                }
            }
            
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit && unit.SfsUnitState != SFSUnitState.Die)
                {
                    unit.TickEnd();
                }
            }
        }
    }
}
