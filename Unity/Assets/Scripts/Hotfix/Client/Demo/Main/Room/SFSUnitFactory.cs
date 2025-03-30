using Unity.Mathematics;

namespace ET.Client
{
    [FriendOf(typeof(SFSUnit))]
    [FriendOfAttribute(typeof(ET.Client.SkillComponent))]
    public static class SFSUnitFactory
    {
        public static void CreateHero(BattleRoom room, SFSUnitInfo info, bool isLocalPlayer)
        {
            SFSUnitComponent component = room.GetComponent<SFSUnitComponent>();
            SFSUnit unit = component.AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            unit.Position = info.Position;
            unit.Rotation = info.Forward;
            unit.SfsUnitType = info.Type;
            unit.SfsUnitCamp = info.Camp;
            unit.SfsUnitState = info.State;
            unit.AddComponent<SkillComponent, SFSUnit>(unit);
            unit.AddComponent<BuffComponent, SFSUnit>(unit);
            if (isLocalPlayer)
                component.MyUnit = unit;
            // Add UnitView, Animator, Camera
            EventSystem.Instance.Publish(room.Root(), new CreateSFSUnit()
            {
                unit = unit,
                IsLocalPlayer = isLocalPlayer
            });
        }

        public static void CreateProjectile(BattleRoom room, SFSUnitInfo info)
        {
            SFSUnit unit = room.GetComponent<SFSUnitComponent>().AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            unit.Position = info.Position;
            unit.Rotation = info.Forward;
            unit.SfsUnitType = info.Type;
            unit.SfsUnitCamp = info.Camp;
            unit.SfsUnitState = info.State;
            unit.Speed = math.forward(unit.Rotation);
            // Add UnitView
            EventSystem.Instance.Publish(room.Root(), new CreateSFSProjectile()
            {
                unit = unit,
            });
        }

        public static void CreateWithReconnectInfo(BattleRoom room, SFSReconnectInfo info, bool isLocalPlayer)
        {
            SFSUnitComponent component = room.GetComponent<SFSUnitComponent>();
            SFSUnit unit = component.AddChildWithId<SFSUnit, BattleRoom>(info.UnitId, room);
            unit.Position = info.Position;
            unit.Rotation = info.Forward;
            unit.SfsUnitType = info.Type;
            unit.SfsUnitCamp = info.Camp;
            unit.SfsUnitState = info.State;
            unit.Speed = info.Speed;
            if (info.Type == SFSUnitType.Player)
            {
                if (isLocalPlayer)
                    component.MyUnit = unit;
                unit.AddComponent<SkillComponent, SFSUnit>(unit);
                unit.AddComponent<BuffComponent, SFSUnit>(unit);
                unit.SfsUnitState = info.State;
                unit.Duration = info.UnitStateDuration;
                unit.HP = info.HP;
                unit.GetComponent<SkillComponent>().State = info.SkillState;
                unit.GetComponent<SkillComponent>().Duration = info.SkillDuration;

                EventSystem.Instance.Publish(room.Root(), new CreateSFSUnit()
                {
                    unit = unit,
                    IsLocalPlayer = isLocalPlayer
                });
            }
            else
            {
                EventSystem.Instance.Publish(room.Root(), new CreateSFSProjectile()
                {
                    unit = unit,
                });
            }
        }
    }
}
