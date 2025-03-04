using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class L2M_CreateBattleRoomHandler : MessageHandler<Scene, L2M_CreateBattleRoom, M2L_CreateBattleRoom>
    {
        protected override async ETTask Run(Scene root, L2M_CreateBattleRoom request, M2L_CreateBattleRoom response)
        {
            Fiber fiber = root.Fiber();
            int fiberId = await FiberManager.Instance.Create(SchedulerType.ThreadPool, fiber.Zone, SceneType.SFSRoom, "SFSRoom");
            ActorId roomRootActorId = new(fiber.Process, fiberId);

            // 发送消息给房间纤程，初始化
            M2Room_Init message = M2Room_Init.Create();
            message.PlayerId.AddRange(request.PlayerId);
            await root.GetComponent<MessageSender>().Call(roomRootActorId, message);
			
            response.ActorId = roomRootActorId;
            await ETTask.CompletedTask;
        }
    }
}