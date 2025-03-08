namespace ET.Client
{
    [ComponentOf(typeof(BattleRoom))]
    public class SFSOperaComponent : Entity, IAwake<PlayerInput>, IUpdate
    {
        public EntityRef<PlayerInput> PlayerInput;
    }
}
