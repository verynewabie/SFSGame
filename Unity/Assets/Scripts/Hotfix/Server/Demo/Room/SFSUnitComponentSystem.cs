namespace ET.Server
{

    [EntitySystemOf(typeof(SFSUnitComponent))]
    [FriendOf(typeof(SFSUnitComponent))]
    public static partial class SFSUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnitComponent self)
        {

        }

        public static void Tick(this SFSUnitComponent self)
        {
            foreach (var info in self.unitToCreate)
            {
                SFSUnitFactory.CreateProjectile(self.GetParent<BattleRoom>(), info);
            }
            self.unitToCreate.Clear();

            foreach (var id in self.unitToDelete)
            {
                self.RemoveChild(id);
            }

            if (self.unitToDelete.Count > 0)
            {
                Room2C_DeleteUnit msg = Room2C_DeleteUnit.Create();
                msg.CmdType = SFSCmdType.DeleteUnitCmd;
                msg.UnitToDelete.AddRange(self.unitToDelete);
                EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
                {
                    Cmd = msg
                });
            }
            self.unitToDelete.Clear();
            
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit)
                {
                    unit.Tick();
                    unit.TickEnd();
                }
            }
        }

        public static void AddUnitToCreate(this SFSUnitComponent self, SFSUnitInfo unitInfo)
        {
            self.unitToCreate.Add(unitInfo);
        }

        public static void AddUnitToDelete(this SFSUnitComponent self, long unitId)
        {
            self.unitToDelete.Add(unitId);
        }
    }
}