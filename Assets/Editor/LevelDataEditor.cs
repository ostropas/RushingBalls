    using Data;
    using UnityEngine;

    namespace Editor
    {
        [UnityEditor.CustomEditor(typeof(LevelData))]
        class LevelDataEditor : UnityEditor.Editor {
            public override void OnInspectorGUI() {
                if (GUILayout.Button("Open edit level"))
                {
                    LevelEditorWindow.ShowWindow(target as LevelData);
                }
            }
        }
    }
