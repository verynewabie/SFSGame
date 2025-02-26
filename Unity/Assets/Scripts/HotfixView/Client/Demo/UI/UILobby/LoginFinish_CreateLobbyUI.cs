namespace ET.Client
{
	[Event(SceneType.Demo)]
	public class LoginFinish_CreateLobbyUI: AEvent<Scene, LoginFinish>
	{
		protected override async ETTask Run(Scene scene, LoginFinish args)
		{
			UI ui = await UIHelper.Create(scene, UIType.UILobby, UILayer.Mid);
			var cmpt = ui.GetComponent<UILobbyComponent>();
			C2G_GetRoomList request = C2G_GetRoomList.Create();
			G2C_GetRoomList response = await scene.GetComponent<ClientSenderComponent>()
					.Call(request) as G2C_GetRoomList;
			cmpt.ShowRoomPreview(response.RoomId, response.RoomHolderName, response.PlayerNum);
		}
	}
}
