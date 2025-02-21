using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHintComponent : Entity, IAwake
    {
        public Text hintText;
        public Button closeBtn;
        public GameObject root;
    }
}
