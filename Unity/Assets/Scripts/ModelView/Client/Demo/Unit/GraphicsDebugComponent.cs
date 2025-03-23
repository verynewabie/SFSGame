using UnityEngine;

namespace ET.Client
{

    [ComponentOf(typeof(BattleRoom))]
    public class GraphicsDebugComponent : Entity, IAwake
    {
        public LineRenderer[] LineRenderer;
    }
}
