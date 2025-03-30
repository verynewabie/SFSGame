using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [ComponentOf(typeof(UI))]
    public class UIReplayComponent : Entity, IAwake
    {
        public MonoPrefabLoader ReplayInfoLoader;
        public GameObject ReplayListRoot;
        public Button GetReplayButton;
    }
}
