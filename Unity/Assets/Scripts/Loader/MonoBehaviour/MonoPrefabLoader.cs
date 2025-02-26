using UnityEngine;

namespace ET
{
    
    public class MonoPrefabLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject monoPrefab;

        public GameObject SpawnGameObject()
        {
            return Instantiate(monoPrefab, transform);
        }
        
    }
}
