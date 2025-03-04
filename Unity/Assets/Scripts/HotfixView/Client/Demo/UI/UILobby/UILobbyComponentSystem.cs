using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILobbyComponent))]
    [FriendOf(typeof(UILobbyComponent))]
    public static partial class UILobbyComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILobbyComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.startGame = rc.Get<GameObject>("StartGame").GetComponent<Button>();
            self.createRoom = rc.Get<GameObject>("CreateRoom").GetComponent<Button>();
            self.enterRoom = rc.Get<GameObject>("EnterRoom").GetComponent<Button>();
            self.leaveRoom = rc.Get<GameObject>("LeaveRoom").GetComponent<Button>();

            self.roomDetail = rc.Get<GameObject>("RoomDetail");
            self.roomList = rc.Get<GameObject>("RoomList");
            self.roomLoader = self.roomList.GetComponent<MonoPrefabLoader>();

            self.playerLoader = rc.Get<GameObject>("PlayerLoader").GetComponent<MonoPrefabLoader>();

            self.startGame.onClick.AddListener(() => self.StartGame());
            self.createRoom.onClick.AddListener(() => self.CreateRoom().Coroutine());
            self.enterRoom.onClick.AddListener(() => self.EnterRoom().Coroutine());
            self.leaveRoom.onClick.AddListener(() => self.LeaveRoom());
            
            self.inputRoomId = rc.Get<GameObject>("InputRoomId").GetComponent<Text>();
            
            self.roomId = new List<long>();
            
            self.enterMap = rc.Get<GameObject>("EnterMap");
            self.enterMap.GetComponent<Button>().onClick.AddListener(() => { self.EnterMap().Coroutine(); });
        }

        public static void ShowRoomPreview(this UILobbyComponent self, List<long> roomId, List<string> ownerName, List<int> nowNum)
        {
            self.roomDetail.SetActive(false);
            self.roomList.SetActive(true);
            
            self.createRoom.gameObject.SetActive(true);
            self.enterRoom.gameObject.SetActive(true);
            self.leaveRoom.gameObject.SetActive(false);
            self.startGame.gameObject.SetActive(false);
            
            int num = roomId.Count;
            for (int i = 0; i < num; i++)
            {
                var roomPreview = self.AddChildWithId<RoomPreview, MonoPrefabLoader>(roomId[i], self.roomLoader);
                roomPreview.ShowRoomPreview(ownerName[i], nowNum[i]);
                self.roomId.Add(roomId[i]);
            }
        }

        private static void ShowRoomDetail(this UILobbyComponent self, long roomHolderId, List<long> playerId, List<string> playerName)
        {
            self.roomDetail.SetActive(true);
            self.roomList.SetActive(false);
            
            long myId = self.Root().GetComponent<PlayerComponent>().MyId;
            
            self.createRoom.gameObject.SetActive(false);
            self.enterRoom.gameObject.SetActive(false);
            self.leaveRoom.gameObject.SetActive(true);
            self.startGame.gameObject.SetActive(myId == roomHolderId);
            
            int num = playerId.Count;
            for (int i = 0; i < num; i++)
            {
                var playerPreview = self.AddChildWithId<PlayerPreview, MonoPrefabLoader>(playerId[i], self.playerLoader);
                playerPreview.ShowPlayerPreview(playerId[i] == myId, playerId[i] == roomHolderId, playerName[i]);
            }
        }
        
        public static void PlayerEnterRoom(this UILobbyComponent self, long playerId, string name)
        {
            var playerPreview = self.AddChildWithId<PlayerPreview, MonoPrefabLoader>(playerId, self.playerLoader);
            playerPreview.ShowPlayerPreview(false, false, name);
        }
        
        private static void StartGame(this UILobbyComponent self)
        {
            StartGameHelper.StartGame(self.Root());
        }

        private static async ETTask CreateRoom(this UILobbyComponent self)
        {
            var root = self.Root();

            C2G_CreateRoom request = C2G_CreateRoom.Create();
            var playerCmpt = root.GetComponent<PlayerComponent>();
            request.PlayerId = playerCmpt.MyId;
            var response = await root.GetComponent<ClientSenderComponent>().Call(request) as G2C_CreateRoom;

            playerCmpt.RoomId = response.RoomId;

            // ShowRoomInfo
            self.ShowRoomDetail(playerCmpt.MyId, 
                new List<long> { playerCmpt.MyId }, 
                new List<string> { playerCmpt.Account });
        }

        private static async ETTask EnterRoom(this UILobbyComponent self)
        {
            var root = self.Root();
            bool succ = int.TryParse(self.inputRoomId.text, out int index);
            if (index < 0 || index >= self.roomId.Count || !succ)
            {
                EventSystem.Instance.PublishAsync(root, new ShowUIHint
                {
                    hint = "请输入合法的房间下标",
                    showCloseBtn = true
                }).Coroutine();
                return;
            }
            
            C2G_EnterRoom request = C2G_EnterRoom.Create();
            request.PlayerId = root.GetComponent<PlayerComponent>().MyId;
            request.RoomId = self.roomId[index];
            var response = await root.GetComponent<ClientSenderComponent>().Call(request) as G2C_EnterRoom;
            self.ShowRoomDetail(response.RoomHolderId, response.PlayerId, response.PlayerName);
            root.GetComponent<PlayerComponent>().RoomId = request.RoomId;
        }

        private static void LeaveRoom(this UILobbyComponent self)
        {
            // TODO
        }
        
        private static async ETTask EnterMap(this UILobbyComponent self)
        {
            Scene root = self.Root();
            await EnterMapHelper.EnterMapAsync(root);
            await UIHelper.Remove(root, UIType.UILobby);
        }
    }
}