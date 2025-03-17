namespace ET.Server
{

    [EntitySystemOf(typeof(SFSUnitComponent))]
    public static partial class SFSUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnitComponent self)
        {
            
        }

        public static void Tick(this SFSUnitComponent self)
        {
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