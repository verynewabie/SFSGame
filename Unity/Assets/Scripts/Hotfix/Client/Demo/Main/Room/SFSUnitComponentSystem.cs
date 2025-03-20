namespace ET.Client
{

    [EntitySystemOf(typeof(SFSUnitComponent))]
    public static partial class SFSUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnitComponent self)
        {
            
        }

        public static void Tick(this SFSUnitComponent self, bool isInChaseFrameState)
        {
            if (isInChaseFrameState)
            {
                SFSUnit unit = self.MyUnit;
                unit.Tick();
                unit.TickEnd();
                return;
            }
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit)
                {
                    unit.Tick();
                    unit.TickEnd();
                }
            }
        }
    }
}
