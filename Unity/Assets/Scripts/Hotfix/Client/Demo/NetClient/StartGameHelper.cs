namespace ET.Client
{
    public static class StartGameHelper
    {
        public static void StartGame(Scene root)
        {
            long roomId = root.GetComponent<PlayerComponent>().RoomId;
            C2L_StartGame startGame = C2L_StartGame.Create();
            startGame.RoomId = roomId;
            root.GetComponent<ClientSenderComponent>().Send(startGame);
        }
    }
}
