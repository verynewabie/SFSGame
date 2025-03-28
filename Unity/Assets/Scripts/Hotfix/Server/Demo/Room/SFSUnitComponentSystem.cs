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
                SFSUnitFactory.CreateProjectile(self.GetParent<BattleRoom>(), info.Info, info.BelongToUnit);
                Log.Error($"Create Projectile In {info.Info.Position.ToString()}");
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
                    if (unit.SfsUnitState != SFSUnitState.Die)
                        unit.Tick();
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

        public static void TickEnd(this SFSUnitComponent self)
        {
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit)
                {
                    if (unit.SfsUnitState != SFSUnitState.Die)
                        unit.TickEnd();
                    else if (unit.SfsUnitState == SFSUnitState.Die
                             && unit.Duration > 0)
                    {
                        unit.Duration = 0;
                        unit.TickEnd();
                    }
                }
            }
        }

        public static void AddUnitToCreate(this SFSUnitComponent self, SFSUnitInfo unitInfo, SFSUnit unit)
        {
            self.unitToCreate.Add(new ProjectileInfo
            {
                Info = unitInfo,
                BelongToUnit = unit,
            });
        }

        public static void AddUnitToDelete(this SFSUnitComponent self, long unitId)
        {
            self.unitToDelete.Add(unitId);
        }

        public static bool IsRedAllDie(this SFSUnitComponent self)
        {
            int redCnt = 0;
            int redDie = 0;
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit)
                {
                    if (unit.SfsUnitType == SFSUnitType.Projectile
                        || unit.SfsUnitCamp == SFSUnitCamp.Blue)
                        continue;
                    if (unit.SfsUnitState == SFSUnitState.Die)
                        redDie++;
                    redCnt++;
                }
            }

            return redCnt != 0 && redCnt == redDie;
        }
        
        public static bool IsBlueAllDie(this SFSUnitComponent self)
        {
            int blueCnt = 0;
            int blueDie = 0;
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit)
                {
                    if (unit.SfsUnitType == SFSUnitType.Projectile
                        || unit.SfsUnitCamp == SFSUnitCamp.Red)
                        continue;
                    if (unit.SfsUnitState == SFSUnitState.Die)
                        blueDie++;
                    blueCnt++;
                }
            }

            return blueCnt != 0 && blueCnt == blueDie;
        }
    }
}