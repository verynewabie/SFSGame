namespace ET.Server
{
    [EntitySystemOf(typeof(SFSUnitComponent))]
    [FriendOf(typeof(SFSUnitComponent))]
    [FriendOf(typeof(SFSUnit))]
    [FriendOfAttribute(typeof(ET.Server.BuffComponent))]
    [FriendOfAttribute(typeof(ET.Server.SkillComponent))]
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
            // Room2C_DebugInfo debugInfo = Room2C_DebugInfo.Create();
            // debugInfo.CmdType = SFSCmdType.DebugInfoCmd;
            // foreach (var entity in self.Children.Values)
            // {
            //     if (entity is SFSUnit unit)
            //     {
            //         debugInfo.Pos.Add(unit.Position);
            //         debugInfo.Radius.Add(unit.SfsUnitType == SFSUnitType.Player ? 1f : 0.25f);
            //     }
            // }
            // EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
            // {
            //     Cmd = debugInfo
            // });
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

        public static Room2C_NotifyReconnectInfo Reconnect(this SFSUnitComponent self)
        {
            Room2C_NotifyReconnectInfo info = Room2C_NotifyReconnectInfo.Create();
            foreach (var entity in self.Children.Values)
            {
                if (entity is SFSUnit unit)
                {
                    SFSReconnectInfo msg = SFSReconnectInfo.Create();
                    msg.Position = unit.Position;
                    msg.Camp = unit.SfsUnitCamp;
                    msg.Forward = unit.Rotation;
                    msg.State = unit.SfsUnitState;
                    msg.Type = unit.SfsUnitType;
                    msg.UnitId = unit.Id;
                    msg.UnitStateDuration = unit.Duration;
                    msg.HP = unit.HP;
                    msg.Speed = unit.Speed;
                    info.Units.Add(msg);
                    if (msg.Type == SFSUnitType.Player)
                    {
                        msg.SkillState = unit.GetComponent<SkillComponent>().State;
                        msg.SkillDuration = unit.GetComponent<SkillComponent>().Duration;
                    }
                }
            }
            return info;
        }
        
    }
}