using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [ChildOf(typeof(UILobbyComponent))]
    public class PlayerPreview : Entity, IAwake<MonoPrefabLoader>, IDestroy
    {
        public GameObject playerPreview;
        public GameObject isMe;
        public GameObject roomHolder;
        public Text name;
    }

    [EntitySystemOf(typeof(PlayerPreview))]
    [FriendOf(typeof(PlayerPreview))]
    public static partial class PlayerPreviewSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.PlayerPreview self, ET.MonoPrefabLoader loader)
        {
            self.playerPreview = loader.SpawnGameObject();
            ReferenceCollector rc = self.playerPreview.GetComponent<ReferenceCollector>();
            self.name = rc.Get<GameObject>("Name").GetComponent<Text>();
            self.isMe = rc.Get<GameObject>("IsMe");
            self.roomHolder = rc.Get<GameObject>("RoomHolder");
        }
        
        [EntitySystem]
        private static void Destroy(this ET.Client.PlayerPreview self)
        {
            UnityEngine.Object.Destroy(self.playerPreview);
        }

        public static void ShowPlayerPreview(this ET.Client.PlayerPreview self, bool isMe, bool isRoomHolder, string name)
        {
            self.isMe.SetActive(isMe);
            self.roomHolder.SetActive(isRoomHolder);
            self.name.text = name;
        }
    }
}
