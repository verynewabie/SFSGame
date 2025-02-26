using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ET
{

    [CustomEditor(typeof(MonoPrefabLoader))]
    public class MonoPrefabLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // 绘制默认的Inspector界面
            base.OnInspectorGUI();

            // 获取当前脚本组件
            MonoPrefabLoader loader = (MonoPrefabLoader)target;

            // 添加一个按钮来创建实例
            if (GUILayout.Button("SpawnPrefab"))
            {
                if (Application.isPlaying)
                    return;
                // 在编辑模式下实例化Prefab
                loader.SpawnGameObject();
                // 标记场景已更改
                EditorSceneManager.MarkSceneDirty(loader.gameObject.scene);
            }
            
            if (GUILayout.Button("ClearGameObject"))
            {
                if (Application.isPlaying)
                    return;
                for (int i = loader.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(loader.transform.GetChild(i).gameObject);
                }
                // 标记场景已更改
                EditorSceneManager.MarkSceneDirty(loader.gameObject.scene);
            }
        }
    }
}
