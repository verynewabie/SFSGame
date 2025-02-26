namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class PlayerEnterRoomHandler: AEvent<Scene, PlayerEnterRoom>
    {
        protected override async ETTask Run(Scene scene, PlayerEnterRoom args)
        {
            var cmpt = scene.GetComponent<UIComponent>().Get(UIType.UILobby).GetComponent<UILobbyComponent>();
            cmpt.PlayerEnterRoom(args.playerId, args.name);
        }
    }
}