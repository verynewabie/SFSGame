using Unity.Mathematics;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSUnit))]
    [FriendOf(typeof(SFSUnit))]
    [FriendOf(typeof(SFSComponent))]
    public static partial class SFSUnitSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnit self, BattleRoom room)
        {
            self.BattleRoom = room;
        }

        public static void Tick(this SFSUnit self)
        {
            if (self.SfsUnitState != SFSUnitState.Abnormal)
            {
                if (!self.Speed.MyEquals(float3.zero))
                {
                    self.Rotation = quaternion.LookRotation(self.Speed, math.up());
                }
                self.Position += self.Speed * SFSConstValue.UpdateIntervalFloat;
            }
            else
            {
                self.Duration--;
                if (self.Duration == 0)
                {
                    self.SfsUnitState = SFSUnitState.Free;
                }
            }

            if (self.SfsUnitType == SFSUnitType.Player)
            {
                self.GetComponent<SkillComponent>().Tick();
                self.GetComponent<BuffComponent>().Tick();
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
            self.Speed = float3.zero;
            // Log.Error($"Frame {moveCmd.FrameId} Pos: {moveCmd.Pos.ToString()}");
            
            // State
            StateCmd stateCmd = StateCmd.Create();
            stateCmd.CmdType = SFSCmdType.StateCmd;
            stateCmd.UnitId = self.Id;
            stateCmd.State = self.SfsUnitState;
            stateCmd.Duration = self.Duration;
            stateCmd.FrameId = sfsCmpt.CurrentFrame;
            
            self.HistoryState[sfsCmpt.CurrentFrame] = stateCmd;
            
            // Attribute
            AttributeCmd attributeCmd = AttributeCmd.Create();
            attributeCmd.CmdType = SFSCmdType.AttributeCmd;
            attributeCmd.UnitId = self.Id;
            attributeCmd.HP = self.HP;
            attributeCmd.FrameId = sfsCmpt.CurrentFrame;
            
            self.HistoryAttribute[sfsCmpt.CurrentFrame] = attributeCmd;
            
            self.GetComponent<SkillComponent>().TickEnd();
            self.GetComponent<BuffComponent>().TickEnd();
        }

        public static void HandleCmd(this SFSUnit self, MoveCmd moveCmd)
        {
            if (!moveCmd.FromClient)
            {
                self.Position = moveCmd.Pos;
                self.Rotation = moveCmd.Rot;
            }
            else
                self.Speed = moveCmd.Speed;
        }

        public static bool CheckConsistency(this SFSUnit self, int frame, MoveCmd moveCmd)
        {
            if (!self.HistoryMoveState.ContainsKey(frame))
                return false;
                
            MoveCmd target = self.HistoryMoveState[frame];
            return target.Pos.MyEquals(moveCmd.Pos) &&
                    target.Rot.MyEquals(moveCmd.Rot) &&
                    target.Speed.MyEquals(moveCmd.Speed);
        }

        public static void Rollback(this SFSUnit self, MoveCmd moveCmd)
        {
            self.Position = moveCmd.Pos;
            self.Rotation = moveCmd.Rot;
            self.Speed = moveCmd.Speed;
        }
        
        public static void HandleCmd(this SFSUnit self, StateCmd stateCmd)
        {
            self.SfsUnitState = stateCmd.State;
            self.Duration = stateCmd.Duration;
        }
        
        public static bool CheckConsistency(this SFSUnit self, int frame, StateCmd stateCmd)
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

        public static void Rollback(this SFSUnit self, StateCmd stateCmd)
        {
            self.SfsUnitState = stateCmd.State;
            self.Duration = stateCmd.Duration;
        }
        
        public static void HandleCmd(this SFSUnit self, AttributeCmd attributeCmd)
        {
            self.HP = attributeCmd.HP;
        }
        
        public static bool CheckConsistency(this SFSUnit self, int frame, AttributeCmd attributeCmd)
        {
            if (!self.HistoryMoveState.ContainsKey(frame))
                return false;
            AttributeCmd target = self.HistoryAttribute[frame];
            return target.HP == attributeCmd.HP;
        }

        public static void Rollback(this SFSUnit self, AttributeCmd attributeCmd)
        {
            self.HP = attributeCmd.HP;
        }
    }
}
