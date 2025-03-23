namespace ET.Server
{

    [EntitySystemOf(typeof(SFSUnitComponent))]
    [FriendOf(typeof(SFSUnitComponent))]
    [FriendOf(typeof(SFSUnit))]
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
                Log.Error($"Create Projectile In {info.Position.ToString()}");
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

            // Send Debug Info
            Room2C_DebugInfo debugInfo = Room2C_DebugInfo.Create();
            debugInfo.CmdType = SFSCmdType.DebugInfoCmd;
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit)
                {
                    debugInfo.Pos.Add(unit.Position);
                    debugInfo.Radius.Add(unit.SfsUnitType == SFSUnitType.Player ? 1f : 0.25f);
                }
            }
            EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
            {
                Cmd = debugInfo
            });
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