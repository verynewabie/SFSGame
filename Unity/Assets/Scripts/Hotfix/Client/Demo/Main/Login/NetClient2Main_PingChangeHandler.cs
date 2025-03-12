namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class NetClient2Main_PingChangeHandler : MessageHandler<Scene,NetClient2Main_PingChange>
    {
        protected override async ETTask Run(Scene root, NetClient2Main_PingChange message)
        {
            await ETTask.CompletedTask;
            var room = root.GetComponent<BattleRoom>();
            if (room == null)
                return;
            room.GetComponent<SFSComponent>().ChangePing(message.NewPing);
        }
    }
}
