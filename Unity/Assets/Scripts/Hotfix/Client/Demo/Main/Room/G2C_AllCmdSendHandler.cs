namespace ET.Client
{

    [MessageHandler(SceneType.Demo)]
    public class G2C_AllCmdSendHandler  : MessageHandler<Scene, G2C_AllCmdSend>
    {
        protected override async ETTask Run(Scene root, G2C_AllCmdSend message)
        {
            await ETTask.CompletedTask;
            root.GetComponent<ObjectWait>().Notify(new Wait_AllCmdSend());
        }
    }
}
