using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(GraphicsDebugComponent))]
    [FriendOf(typeof(GraphicsDebugComponent))]
    public static partial class GraphicsDebugComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GraphicsDebugComponent self)
        {
            GameObject debug = GameObject.Find("Debug");
            int count = debug.transform.childCount;
            self.LineRenderer = new LineRenderer[count];
            for (int i = 0; i < count; i++)
            {
                self.LineRenderer[i] = debug.transform.GetChild(i).GetComponent<LineRenderer>();
            }
        }

        public static void Render(this GraphicsDebugComponent self, ShowDebugInfo info)
        {
            for (int i = 0; i < info.Pos.Count; i++)
            {
                Render(self.LineRenderer[i], info.Pos[i], info.Radius[i]);
            }
        }

        private static void Render(LineRenderer render, Vector3 pos, float radius)
        {
            Vector3[] positions = new Vector3[]
            {
                new Vector3(pos.x + radius, 1, pos.z),
                new Vector3(pos.x, 1, pos.z + radius),
                new Vector3(pos.x - radius, 1, pos.z),
                new Vector3(pos.x, 1, pos.z - radius),
            };
            render.positionCount = positions.Length;
            render.SetPositions(positions);
        }
    }
}
