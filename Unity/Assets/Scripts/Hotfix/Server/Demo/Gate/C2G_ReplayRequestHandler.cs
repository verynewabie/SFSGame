using System.Collections.Generic;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Gate)]
    [FriendOf(typeof(OneGameInfo))]
    public class C2G_ReplayRequestHandler : MessageSessionHandler<C2G_ReplayRequest, G2C_ReplayResponse>
    {
        protected override async ETTask Run(Session session, C2G_ReplayRequest request, G2C_ReplayResponse response)
        {
            DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
            List<OneGameInfo> oneGameInfos = await dbComponent.Query<OneGameInfo>(info => info.BattleId == request.BattleId);
            if (oneGameInfos.Count > 0)
            {
                OneGameInfo info = oneGameInfos[0];
                response.Units = info.Units;
            }
            else
            {
                response.Error = ErrorCode.ERR_ReplayDataNotFound;
            }
        }
    }
}
