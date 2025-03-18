namespace ET.Server
{

    [MessageHandler(SceneType.SFSRoom)]
    public class C2Room_MoveCmdHandler : MessageHandler<Scene, MoveCmd>
    {
        protected override async ETTask Run(Scene root, MoveCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
    
    [MessageHandler(SceneType.SFSRoom)]
    public class C2Room_SkillCmdHandler : MessageHandler<Scene, SkillCmd>
    {
        protected override async ETTask Run(Scene root, SkillCmd message)
        {
            root.GetComponent<BattleRoom>().GetComponent<SFSComponent>().AddCmdToHandleQueue(message);
            await ETTask.CompletedTask;
        }
    }
}
