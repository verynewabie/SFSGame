using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIReplayComponent))]
    [FriendOf(typeof(UIReplayComponent))]
    public static partial class UIReplayComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIReplayComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.ReplayListRoot = rc.Get<GameObject>("ReplayList");
            self.ReplayInfoLoader = self.ReplayListRoot.GetComponent<MonoPrefabLoader>();
            
            self.GetReplayButton = rc.Get<GameObject>("GetReplay").GetComponent<Button>();
            self.GetReplayButton.onClick.AddListener(() => { self.GetReplayList().Coroutine(); });
        }

        private static async ETTask GetReplayList(this UIReplayComponent self)
        {
            C2G_GetPlayerBattles request = C2G_GetPlayerBattles.Create();
            G2C_GetPlayerBattles response = await self.Root().GetComponent<ClientSenderComponent>().Call(request)
                    as G2C_GetPlayerBattles;
            self.RefreshReplayList(response.Battles);
        }

        private static void RefreshReplayList(this UIReplayComponent self, List<BattleInfo> battles)
        {
            Transform transform = self.ReplayListRoot.transform;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
            }

            foreach (BattleInfo battle in battles)
            {
                GameObject oneGame = self.ReplayInfoLoader.SpawnGameObject();
                self.SetupOneGame(oneGame, battle.Win, battle.Time, battle.BattleId);
            }
        }

        private static void SetupOneGame(this UIReplayComponent self,
        GameObject target, bool win, long time, long battleId)
        {
            ReferenceCollector rc = target.GetComponent<ReferenceCollector>();

            Text result = rc.Get<GameObject>("Result").GetComponent<Text>();
            Text timeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            Button replayButton = rc.Get<GameObject>("ReplayButton").GetComponent<Button>();

            result.text = win ? "胜利" : "失败";

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(time);
            string formatted = dateTimeOffset.ToLocalTime().ToString("MM/dd HH:mm");
            timeText.text = formatted;

            replayButton.onClick.AddListener(() => self.StartReplay(battleId).Coroutine());
        }

        private static async ETTask StartReplay(this UIReplayComponent self, long battleId)
        {
            C2G_GetOneGameInfo request = C2G_GetOneGameInfo.Create();
            request.BattleId = battleId;
            G2C_GetOneGameInfo response = await self.Root().GetComponent<ClientSenderComponent>()
                    .Call(request) as G2C_GetOneGameInfo;
            Log.Error($"Replay Count: {response.Cmds.Count}");
        }
    }
}
