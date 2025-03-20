using Unity.Mathematics;

namespace ET
{
    public static class CompareTool
    {
        private const float Epsilon = 0.001f;
        public static bool MyEquals(this float2 a, float2 b)
        {
            return math.abs(a.x - b.x) < Epsilon 
                    && math.abs(a.y - b.y) < Epsilon;
        }
        
        public static bool MyEquals(this float3 a, float3 b)
        {
            return math.abs(a.x - b.x) < Epsilon
                    && math.abs(a.y - b.y) < Epsilon
                    && math.abs(a.z - b.z) < Epsilon;
        }

        public static bool MyEquals(this quaternion a, quaternion b)
        {
            return math.abs(a.value.x - b.value.x) < Epsilon
                    && math.abs(a.value.y - b.value.y) < Epsilon
                    && math.abs(a.value.z - b.value.z) < Epsilon
                    && math.abs(a.value.w - b.value.w) < Epsilon;
        }
    }
}
