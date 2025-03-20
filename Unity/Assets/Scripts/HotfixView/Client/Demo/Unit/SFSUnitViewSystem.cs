using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSUnitView))]
    [FriendOf(typeof(SFSUnitView))]
    [FriendOf(typeof(SFSUnit))]
    public static partial class SFSUnitViewSystem
    {
        [EntitySystem]
        private static void Awake(this SFSUnitView self, SFSUnit unit)
        {
            self.Unit = unit;
            self.Room = unit.BattleRoom;
        }

        public static async ETTask InitAsync(this SFSUnitView self)
        {
            string assetName = "Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await self.Room.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>(assetName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{self.Unit.SfsUnitCamp.ToString()}");

            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            self.GameObject = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            self.Transform = self.GameObject.transform;
            self.Transform.position = self.Unit.Position;
            self.Transform.rotation = self.Unit.Rotation;
        }

        public static async ETTask InitProjectile(this SFSUnitView self)
        {
            string assetName = "Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await self.Room.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>(assetName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"Projectile");

            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            self.GameObject = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            self.Transform = self.GameObject.transform;
            self.Transform.position = self.Unit.Position;
            self.Transform.rotation = self.Unit.Rotation;
        }

        [EntitySystem]
        private static void Update(this SFSUnitView self)
        {
            if (self.Transform == null)
                return;
            float lerpSpeed = 5f;
            self.Transform.position = Vector3.Lerp(self.Transform.position, self.Unit.Position,
                lerpSpeed * Time.deltaTime);
            self.Transform.rotation = Quaternion.Slerp(self.Transform.rotation, self.Unit.Rotation,
                lerpSpeed * Time.deltaTime);
        }
        
        [EntitySystem]
        private static void Destroy(this SFSUnitView self)
        {
            self.Transform = null;
            UnityEngine.Object.Destroy(self.GameObject);
            self.GameObject = null;
        }
    }
}
