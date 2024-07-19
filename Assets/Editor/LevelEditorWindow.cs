using System.Collections.Generic;
using Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private LevelData _levelData;
        private int _width;
        private int _height;
        private int _prevWidth;
        private int _prevHeight;
        
        public static void  ShowWindow (LevelData levelData) {
            LevelEditorWindow win = (LevelEditorWindow)GetWindow(typeof(LevelEditorWindow ));
            win.Init(levelData);
            win.Show();
        }

        private void Init(LevelData levelData)
        {
            _levelData = levelData;
            _width = levelData.Field.Count;
            _height = _width > 0 ? levelData.Field[0].Count : 0;
            _prevHeight = _height;
            _prevWidth = _width;
        }
    
        void OnGUI () {
            DrawFieldSize();
        }

        private void DrawFieldSize()
        {
            _width = EditorGUILayout.IntField("Width", _width);
            _height = EditorGUILayout.IntField("Height", _height);
            if ((_prevWidth != _width || _prevHeight != _height) && _width > 0 && _height > 0)
            {
                _levelData.Field = new();
                for (int i = 0; i < _width; i++)
                {
                    _levelData.Field.Add(new List<Field>());
                    for (int j = 0; j < _height; j++)
                    {
                        _levelData.Field[i].Add(new Field());
                    }
                }
                EditorUtility.SetDirty(_levelData);
            }

            _prevHeight = _height;
            _prevWidth = _width;
        }
    }
}