using System.Collections.Generic;

namespace ET.Client
{

    [MessageHandler(SceneType.Demo)]
    public class Room2C_NotifyReconnectInfoHandler  : MessageHandler<Scene,  Room2C_NotifyReconnectInfo>
    {
        protected override async ETTask Run(Scene root, Room2C_NotifyReconnectInfo message)
        {
            // Remove Login UI
            await EventSystem.Instance.PublishAsync(root, new Reconnect());
            
            long playerId = root.GetComponent<PlayerComponent>().MyId;
            List<long> players = new List<long>();
            foreach (var unit in message.Units)
                players.Add(unit.UnitId);
            var room = root.AddComponent<BattleRoom, List<long>>(players);
            room.AddComponent<SFSComponent>();
            // Load Map, 这一步给BattleRoom加了 ResourcesLoaderComponent
            await EventSystem.Instance.PublishAsync(root, new SFSLoadScene
            {
                sceneName = "SFSGame"
            });
            
            room.AddComponent<SFSUnitComponent>();
            
            // Load Units
            foreach (SFSReconnectInfo info in message.Units)
            {
                SFSUnitFactory.CreateWithReconnectInfo(room, info, playerId == info.UnitId);
            }
            // Add SFSOperaComponent And So On
            PlayerInputComponent inputComponent = room.AddComponent<PlayerInputComponent>();
            await EventSystem.Instance.PublishAsync(root, new InitBattleView
            {
                PlayerInputComponent = inputComponent
            });
            // Reconnect Complete, Send Message
            C2Room_ReconnectDone notify = C2Room_ReconnectDone.Create();
            root.GetComponent<ClientSenderComponent>().Send(notify);
        }
    }
}
