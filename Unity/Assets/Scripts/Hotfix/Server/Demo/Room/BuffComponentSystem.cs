namespace ET.Server
{

    [EntitySystemOf(typeof(BuffComponent))]
    [FriendOf(typeof(BuffComponent))]
    public static partial class BuffComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuffComponent self, SFSUnit owner)
        {
            self.Owner = owner;
        }

        public static void AddBuff(this BuffComponent self, SFSBuff buff)
        {
            self.Buff.Add(buff);
            buff.ApplyEffect(self.Owner);
            BuffCmd cmd = BuffCmd.Create();
            cmd.CmdType = SFSCmdType.BuffCmd;
            cmd.UnitId = self.Owner.Id;
            cmd.Type = SFSBuffType.Stun;
            EventSystem.Instance.Publish(self.Root(), new AddCmdToSendQueue
            {
                Cmd = cmd
            });
        }

        public static void Tick(this BuffComponent self)
        {
            for (int i = self.Buff.Count - 1; i >= 0; i--)
            {
                if (self.Buff[i].UpdateBuff())
                {
                    self.Buff[i].RemoveBuff();
                    self.Buff.RemoveAt(i);
                }
            }
        }

        public static void TickEnd(this BuffComponent self)
        {
            
        }
    }
}
