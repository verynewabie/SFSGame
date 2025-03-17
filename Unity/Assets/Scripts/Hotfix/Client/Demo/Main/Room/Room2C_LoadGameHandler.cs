using System.Collections.Generic;

namespace ET.Client
{

    [MessageHandler(SceneType.Demo)]
    public class Room2C_LoadGameHandler : MessageHandler<Scene, Room2C_LoadGame>
    {
        protected override async ETTask Run(Scene root, Room2C_LoadGame message)
        {
            
            await EventSystem.Instance.PublishAsync(root, new ShowUIHint
            {
                showCloseBtn = false,
                hint = "加载游戏中..."
            });
            
            long playerId = root.GetComponent<PlayerComponent>().MyId;
            long roomId = root.GetComponent<PlayerComponent>().RoomId;
            root.GetComponent<SFSRoomsComponent>().RemoveChild(roomId);

            var room = root.AddComponent<BattleRoom, List<long>>(message.PlayerId);
            room.AddComponent<SFSComponent>();
            // Load Map, 这一步给BattleRoom加了 ResourcesLoaderComponent
            await EventSystem.Instance.PublishAsync(root, new SFSLoadScene
            {
                sceneName = "SFSGame"
            });
            
            // Remove Lobby UI
            await EventSystem.Instance.PublishAsync(root, new SFSLoadSceneDone());
            
            var unitComponent = room.AddComponent<SFSUnitComponent>();
            
            // Load Units
            foreach (SFSUnitInfo info in message.UnitInfos)
            {
                SFSUnit unit = SFSUnitFactory.Create(room, info);
                if (playerId == unit.Id)
                    unitComponent.MyUnit = unit;
                // Add UnitView, Animator, Camera
                await EventSystem.Instance.PublishAsync(root, new CreateSFSUnit()
                {
                    unit = unit,
                    IsLocalPlayer = playerId == unit.Id
                });
            }
            // Add SFSOperaComponent And So On
            PlayerInputComponent inputComponent = room.AddComponent<PlayerInputComponent>();
            await EventSystem.Instance.PublishAsync(root, new InitBattleView
            {
                PlayerInputComponent = inputComponent
            });
            // Load Complete, Send Message
            C2Room_LoadGameDone notify = C2Room_LoadGameDone.Create();
            root.GetComponent<ClientSenderComponent>().Send(notify);
        }
    }
}