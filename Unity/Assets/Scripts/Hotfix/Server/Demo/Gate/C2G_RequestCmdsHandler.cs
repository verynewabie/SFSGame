using System.Collections.Generic;

namespace ET.Server
{

    [MessageSessionHandler(SceneType.Gate)]
    [FriendOf(typeof(OneGameInfo))]
    public class C2G_RequestCmdsHandler : MessageSessionHandler<C2G_RequestCmds>
    {
        protected override async ETTask Run(Session session, C2G_RequestCmds message)
        {
            DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
            List<OneGameInfo> gameInfo = await dbComponent.Query<OneGameInfo>(info => info.BattleId == message.BattleId);
            if (gameInfo.Count > 0)
            {
                foreach (Queue<IRoomCmd> cmds in gameInfo[0].Cmds.Values)
                {
                    foreach (var cmd in cmds)
                    {
                        session.Send(cmd);
                    }
                }
            }
            G2C_AllCmdSend hint = G2C_AllCmdSend.Create();
            session.Send(hint);
        }
    }
}
