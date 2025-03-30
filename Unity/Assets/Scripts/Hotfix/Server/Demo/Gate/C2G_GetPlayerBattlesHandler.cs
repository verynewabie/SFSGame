using System.Collections.Generic;

namespace ET.Server
{

    [MessageSessionHandler(SceneType.Gate)]
    [FriendOf(typeof(PlayerGameInfo))]
    public class C2G_GetPlayerBattlesHandler : MessageSessionHandler<C2G_GetPlayerBattles, G2C_GetPlayerBattles>
    {
        protected override async ETTask Run(Session session, C2G_GetPlayerBattles request, G2C_GetPlayerBattles response)
        {
            DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
            List<PlayerGameInfo> playerGame = await dbComponent.Query<PlayerGameInfo>(info => info.PlayerId == request.PlayerId);
            if (playerGame.Count > 0)
            {
                response.Battles = playerGame[0].Battles;
            }
        }
    }
}
