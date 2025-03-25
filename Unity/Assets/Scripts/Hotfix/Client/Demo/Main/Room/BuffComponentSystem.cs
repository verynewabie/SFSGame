namespace ET.Client
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

        private static void AddBuff(this BuffComponent self, SFSBuff buff)
        {
            self.Buff.Add(buff);
            buff.ApplyEffect(self.Owner);
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

        public static void HandleCmd(this BuffComponent self, BuffCmd cmd)
        {
            switch (cmd.Type)
            {
                case SFSBuffType.Stun:
                    self.AddBuff(new StunBuff());
                    break;
                default:
                    Log.Error($"Not handle buff type: {cmd.Type.ToString()}");
                    break;
            }
        }
    }
}