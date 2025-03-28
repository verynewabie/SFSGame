namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                await EventSystem.Instance.PublishAsync(root, new ShowUIHint
                {
                    hint = "账号、密码不能为空",
                    showCloseBtn = true
                });
                return;
            }
            
            root.RemoveComponent<ClientSenderComponent>();
            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();
            
            await EventSystem.Instance.PublishAsync(root, new ShowUIHint
            {
                hint = "登录中",
                showCloseBtn = false
            });
            var response = await clientSenderComponent.LoginAsync(account, password);
            await EventSystem.Instance.PublishAsync(root, new HideUIHint());
            
            if (response.Error == ErrorCode.ERR_LoginPasswordError)
            {
                await EventSystem.Instance.PublishAsync(root, new ShowUIHint
                {
                    hint = "密码错误",
                    showCloseBtn = true
                });
                return;
            }
            if (response.Error == ErrorCode.ERR_PlayerAlreadyOnline)
            {
                await EventSystem.Instance.PublishAsync(root, new ShowUIHint
                {
                    hint = "当前用户已在线",
                    showCloseBtn = true
                });
                return;
            }

            if (response.Error == ErrorCode.ERR_PlayerReconnect)
            {
                await EventSystem.Instance.PublishAsync(root, new ShowUIHint
                {
                    showCloseBtn = false,
                    hint = "断线重连中..."
                });
            }

            var playerCmpt = root.GetComponent<PlayerComponent>();
            playerCmpt.MyId = response.PlayerId;
            playerCmpt.Account = account;

            if (response.Error == ErrorCode.ERR_PlayerReconnect)
                return;
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}