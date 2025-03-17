using Unity.Mathematics;

namespace ET
{
    public static class CompareTool
    {
        public static bool MyEquals(this float2 a, float2 b)
        {
            // 本地浮点运算应该不会出问题
            return a.x == b.x && a.y == b.y;
        }
        
        public static bool MyEquals(this float3 a, float3 b)
        {
            // 本地浮点运算应该不会出问题
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool MyEquals(this quaternion a, quaternion b)
        {
            return a.value.x == b.value.x && a.value.y == b.value.y &&
                    a.value.z == b.value.z && a.value.w == b.value.w;
        }
    }
}
