namespace ET.Client
{
    [ComponentOf(typeof(BattleRoom))]
    public class SFSOperaComponent : Entity, IAwake<PlayerInputComponent>, IUpdate
    {
        private EntityRef<PlayerInputComponent> playerInput;
        public PlayerInputComponent PlayerInput
        {
            get { return playerInput; }
            set { playerInput = value; }
        }
        
        private EntityRef<SFSCameraComponent> camera;
        public SFSCameraComponent Camera
        {
            get { return camera; }
            set { camera = value; }
        }
    }
}
