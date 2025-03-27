using System;
using Unity.Mathematics;

namespace ET.Server
{

    [EntitySystemOf(typeof(SFSUnit))]
    [FriendOf(typeof(SFSUnit))]
    [FriendOf(typeof(SFSComponent))]
    [FriendOf(typeof(ColliderComponent))]
    public static partial class SFSUnitSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnit self, BattleRoom room)
        {
            self.BattleRoom = room;
        }

        public static void Tick(this SFSUnit self)
        {
            if (self.SfsUnitState == SFSUnitState.Free)
            {
                if (!self.Speed.MyEquals(float3.zero))
                {
                    self.Rotation = quaternion.LookRotation(self.Speed, math.up());
                }
                self.Position += self.Speed * SFSConstValue.UpdateIntervalFloat;
            }
            else
            {
                self.Speed = float3.zero;
                self.Duration--;
                if (self.Duration == 0)
                {
                    self.SfsUnitState = SFSUnitState.Free;
                }
            }

            if (self.SfsUnitType == SFSUnitType.Player)
            {
                self.GetComponent<SkillComponent>().Tick();
                self.GetComponent<ColliderComponent>().Tick();
                self.GetComponent<BuffComponent>().Tick();
            }
            else
            {
                self.GetComponent<ColliderComponent>().Tick();
                if (math.abs(self.Position.x) > 15 || math.abs(self.Position.z) > 15)
                {
                    EventSystem.Instance.Publish(self.Root(), new AddUnitToRemove
                    {
                        UnitId = self.Id
                    });
                }
            }
        }

        public static void TickEnd(this SFSUnit self)
        {
            if (self.SfsUnitType == SFSUnitType.Projectile)
                return;
            var sfsCmpt = self.BattleRoom.GetComponent<SFSComponent>();

            // Move
            MoveCmd moveCmd = MoveCmd.Create();
            moveCmd.Pos = self.Position;
            moveCmd.Speed = self.Speed;
            moveCmd.Rot = self.Rotation;
            moveCmd.CmdType = SFSCmdType.MoveCmd;
            moveCmd.UnitId = self.Id;
            moveCmd.FrameId = sfsCmpt.CurrentFrame;

            self.HistoryMoveState[sfsCmpt.CurrentFrame] = moveCmd;
            if (!self.CheckConsistency(sfsCmpt.CurrentFrame - 1, moveCmd))
            {
                EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
                {
                    Cmd = moveCmd
                });
            }
            // Log.Error($"Frame {moveCmd.FrameId} Pos: {moveCmd.Pos.ToString()}");

            // State
            StateCmd stateCmd = StateCmd.Create();
            stateCmd.CmdType = SFSCmdType.StateCmd;
            stateCmd.UnitId = self.Id;
            stateCmd.State = self.SfsUnitState;
            stateCmd.Duration = self.Duration;
            stateCmd.FrameId = sfsCmpt.CurrentFrame;

            self.HistoryState[sfsCmpt.CurrentFrame] = stateCmd;
            if (!self.CheckConsistency(sfsCmpt.CurrentFrame - 1, stateCmd))
            {
                EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
                {
                    Cmd = stateCmd
                });
            }

            // Attribute
            AttributeCmd attributeCmd = AttributeCmd.Create();
            attributeCmd.CmdType = SFSCmdType.AttributeCmd;
            attributeCmd.UnitId = self.Id;
            attributeCmd.HP = self.HP;
            attributeCmd.FrameId = sfsCmpt.CurrentFrame;

            self.HistoryAttribute[sfsCmpt.CurrentFrame] = attributeCmd;
            if (!self.CheckConsistency(sfsCmpt.CurrentFrame - 1, attributeCmd))
            {
                EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
                {
                    Cmd = attributeCmd
                });
            }

            // TODO AddCmdToWholeCmdsBuffer

            self.GetComponent<SkillComponent>().TickEnd();
            self.GetComponent<ColliderComponent>().TickEnd();
            self.GetComponent<BuffComponent>().TickEnd();
        }

        public static void HandleCmd(this SFSUnit self, MoveCmd moveCmd)
        {
            self.Speed = moveCmd.Speed;
        }

        private static bool CheckConsistency(this SFSUnit self, int frame, MoveCmd moveCmd)
        {
            if (!self.HistoryMoveState.ContainsKey(frame))
                return false;
            MoveCmd target = self.HistoryMoveState[frame];
            return target.Pos.MyEquals(moveCmd.Pos) &&
                    target.Rot.MyEquals(moveCmd.Rot) &&
                    target.Speed.MyEquals(moveCmd.Speed);
        }

        private static bool CheckConsistency(this SFSUnit self, int frame, StateCmd stateCmd)
        {
            if (!self.HistoryMoveState.ContainsKey(frame))
                return false;
            StateCmd target = self.HistoryState[frame];
            if (target.State != stateCmd.State)
                return false;
            if (target.State == SFSUnitState.Free)
                return true;
            return target.FrameId - stateCmd.FrameId == stateCmd.Duration - target.Duration;
        }

        private static bool CheckConsistency(this SFSUnit self, int frame, AttributeCmd attributeCmd)
        {
            if (!self.HistoryMoveState.ContainsKey(frame))
                return false;
            AttributeCmd target = self.HistoryAttribute[frame];
            return target.HP == attributeCmd.HP;
        }

        public static void ChangeUnitState(this SFSUnit self, SFSUnitState state, int duration)
        {
            self.SfsUnitState = state;
            self.Duration = duration;
        }

        public static void TakeDamage(this SFSUnit self, int damage)
        {
            self.HP -= damage;
            self.HP = Math.Max(0, self.HP);
            if (self.HP == 0)
            {
                self.SfsUnitState = SFSUnitState.Die;
                self.Duration = 1;
                EventSystem.Instance.Publish(self.Root(), new AddBodyToRemove
                {
                    Body = self.GetComponent<ColliderComponent>().Body
                });
            }
        }
    }
}