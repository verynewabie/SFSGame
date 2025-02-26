using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [EntitySystemOf(typeof(RoomPreview))]
    [FriendOf(typeof(RoomPreview))]
    public static partial class RoomPreviewSystem
    {
        [EntitySystem]
        private static void Awake(this RoomPreview self, MonoPrefabLoader loader)
        {
            self.roomPreview = loader.SpawnGameObject();
            ReferenceCollector rc = self.roomPreview.GetComponent<ReferenceCollector>();
            self.ownerName = rc.Get<GameObject>("OwnerName").GetComponent<Text>();
            self.playerNum = rc.Get<GameObject>("PlayerNum").GetComponent<Text>();
        }

        [EntitySystem]
        private static void Destroy(this RoomPreview self)
        {
            UnityEngine.Object.Destroy(self.roomPreview);
        }

        public static void ShowRoomPreview(this RoomPreview self, string ownerName, int playerNum)
        {
            self.ownerName.text = ownerName;
            self.playerNum.text = playerNum.ToString();
        }
    }
}
