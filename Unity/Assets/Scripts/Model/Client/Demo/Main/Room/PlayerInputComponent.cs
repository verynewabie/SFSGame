using Unity.Mathematics;

namespace ET.Client
{
    [ComponentOf(typeof(BattleRoom))]
    public class PlayerInputComponent : Entity, IAwake
    {
        // TODO Add Dir, From Camera Component
        public bool W_Down;
        public bool W_Press;
        public bool W_Up;
        
        public bool A_Down;
        public bool A_Press;
        public bool A_Up;
        
        public bool S_Down;
        public bool S_Press;
        public bool S_Up;
        
        public bool D_Down;
        public bool D_Press;
        public bool D_Up;
        
        public bool Q_Down;
        public bool Q_Press;
        public bool Q_Up;

        public bool E_Down;
        public bool E_Press;
        public bool E_Up;

        public float2 Forward;
        public float2 Right;

        public long PlayerId;
    }
}
