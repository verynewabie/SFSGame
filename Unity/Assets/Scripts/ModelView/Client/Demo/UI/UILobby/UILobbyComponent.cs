using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ComponentOf(typeof(UI))]
	public class UILobbyComponent : Entity, IAwake
	{
		public GameObject enterMap;
		
		public Button createRoom;
		public Button enterRoom;
		public Button leaveRoom;
		public Button startGame;
		public GameObject roomList;
		public GameObject roomDetail;
		public MonoPrefabLoader roomLoader;
		public MonoPrefabLoader playerLoader;
		public Text inputRoomId;
		public List<long> roomId;
	}
}
