using System.Collections.Generic;

namespace ET.Client
{

    [Event(SceneType.Demo)]
    public class ReplayGameHandler : AEvent<Scene, ReplayGame>
    {
        protected override async ETTask Run(Scene root, ReplayGame arg)
        {
            await EventSystem.Instance.PublishAsync(root, new ShowUIHint
            {
                showCloseBtn = false,
                hint = "加载录像中..."
            });
            
            long playerId = root.GetComponent<PlayerComponent>().MyId;

            List<long> playerIds = new List<long>();
            foreach (var unitInfo in arg.units)
                playerIds.Add(unitInfo.UnitId);
            var room = root.AddComponent<BattleRoom, List<long>>(playerIds);
            room.AddComponent<ReplayComponent>();
            // Load Map, 这一步给BattleRoom加了 ResourcesLoaderComponent
            await EventSystem.Instance.PublishAsync(root, new SFSLoadScene
            {
                sceneName = "SFSGame"
            });
            
            // Remove Replay UI
            await EventSystem.Instance.PublishAsync(root, new RemoveReplayUI());
            
            room.AddComponent<SFSUnitComponent>();
            
            // Load Units
            foreach (SFSUnitInfo info in arg.units)
            {
                SFSUnitFactory.CreateHero(room, info, playerId == info.UnitId);
            }
            
            // Set Cursor
            await EventSystem.Instance.PublishAsync(root, new InitReplayBattleView());
            
            // Load Complete, Send Message
            C2G_RequestCmds request = C2G_RequestCmds.Create();
            request.BattleId = arg.battleId;
            root.GetComponent<ClientSenderComponent>().Send(request);
            
            // Wait All Cmd Send
            await root.GetComponent<ObjectWait>().Wait<Wait_AllCmdSend>();
            await EventSystem.Instance.PublishAsync(root, new HideUIHint());
            room.GetComponent<ReplayComponent>().StartSync(TimeInfo.Instance.ClientFrameTime());
        }
    }
}
