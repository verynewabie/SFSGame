namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Room2C_MoveCmdHandler : MessageHandler<Scene, MoveCmd>
    {
        protected override async ETTask Run(Scene root, MoveCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.Demo)]
    public class Room2C_SkillCmdHandler : MessageHandler<Scene, SkillCmd>
    {
        protected override async ETTask Run(Scene root, SkillCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.Demo)]
    public class Room2C_DeleteUnitHandler : MessageHandler<Scene, Room2C_DeleteUnit>
    {
        protected override async ETTask Run(Scene root, Room2C_DeleteUnit message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.Demo)]
    public class Room2C_DebugInfoHandler : MessageHandler<Scene, Room2C_DebugInfo>
    {
        protected override async ETTask Run(Scene root, Room2C_DebugInfo message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.Demo)]
    public class StateCmdHandler : MessageHandler<Scene, StateCmd>
    {
        protected override async ETTask Run(Scene root, StateCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.Demo)]
    public class BuffCmdHandler : MessageHandler<Scene, BuffCmd>
    {
        protected override async ETTask Run(Scene root, BuffCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.Demo)]
    public class AttributeCmdHandler : MessageHandler<Scene, AttributeCmd>
    {
        protected override async ETTask Run(Scene root, AttributeCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.Demo)]
    public class OneFrameEndCmdHandler : MessageHandler<Scene, Room2C_OneFrameEnd>
    {
        protected override async ETTask Run(Scene root, Room2C_OneFrameEnd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().OneFrameEndHandler(message);
            await ETTask.CompletedTask;
        }
    }
}