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
}
