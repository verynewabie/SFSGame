using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(SFSUnitView))]
    [FriendOfAttribute(typeof(ET.Client.SFSUnitView))]
    [FriendOfAttribute(typeof(ET.SFSUnit))]
    public static partial class SFSUnitViewSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.SFSUnitView self, SFSUnit unit)
        {
            self.Unit = unit;
            self.Room = unit.BattleRoom;
        }

        public static async ETTask InitAsync(this SFSUnitView self)
        {
            string assetName = "Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await self.Room.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>(assetName);
            GameObject prefab = bundleGameObject.Get<GameObject>($"{self.Unit.UnitCamp.ToString()}");
            
            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            self.GameObject = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            self.Transform = self.GameObject.transform;
            self.Transform.position = self.Unit.Position;
            self.Transform.rotation = self.Unit.Rotation;
        }
    }
}
