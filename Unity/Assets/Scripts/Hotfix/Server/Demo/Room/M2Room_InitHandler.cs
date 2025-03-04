using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.SFSRoom)]
    public class M2Room_InitHandler: MessageHandler<Scene, M2Room_Init, Room2M_Init>
    {
        protected override async ETTask Run(Scene root, M2Room_Init request, Room2M_Init response)
        {
            BattleRoom room = root.AddComponent<BattleRoom, List<long>>(request.PlayerId);
            room.AddComponent<SFSUnitComponent>();
            var roomPlayerComponent = room.AddComponent<SFSRoomPlayerComponent>();
            Room2C_LoadGame message = Room2C_LoadGame.Create();
            for (int i = 0; i < request.PlayerId.Count; i++)
            {
                long id = request.PlayerId[i];
                roomPlayerComponent.AddChildWithId<SFSRoomPlayer>(id);
                SFSUnitInfo info = SFSUnitInfo.Create();
                info.UnitId = id;
                info.Camp = i * 2 < request.PlayerId.Count ? UnitCamp.Home : UnitCamp.Away;
                message.UnitInfos.Add(info);
                // Create Unit
                // register unit location
                SFSUnit unit = SFSUnitFactory.Create(room, info);
                // TODO 是否需要
                await unit.AddLocation(LocationType.Unit);
            }
            // broadcast load units
            message.PlayerId = request.PlayerId;
            room.Broadcast(message);
            await ETTask.CompletedTask;
        }
    }
}