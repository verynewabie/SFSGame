using System.Collections.Generic;


namespace ET.Server
{
	public static partial class AddressHelper
	{
		public static StartSceneConfig GetGate(int zone, string account)
		{
			// ulong hash = (ulong)account.GetLongHashCode();
			
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
			
			// return zoneGates[(int)(hash % (ulong)zoneGates.Count)];
			return zoneGates[0];
		}

		public static StartSceneConfig GetLobby(int zone)
		{
			return StartSceneConfigCategory.Instance.Lobbys[zone];
		}
	}
}
