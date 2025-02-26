using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [ChildOf(typeof(UILobbyComponent))]
    public class RoomPreview : Entity, IAwake<MonoPrefabLoader>, IDestroy
    {
        public GameObject roomPreview;
        public Text ownerName;
        public Text playerNum;
    }
}
