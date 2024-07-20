using System;
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
        private Texture _triangleTex;
        
        public static void  ShowWindow (LevelData levelData) {
            LevelEditorWindow win = (LevelEditorWindow)GetWindow(typeof(LevelEditorWindow ));
            win.Init(levelData);
            win.Show();
        }

        private void Init(LevelData levelData)
        {
            _levelData = levelData;
            _width = levelData.Field.Count;
            _height = _width > 0 ? levelData.Field[0].Column.Count : 0;
            _prevHeight = _height;
            _prevWidth = _width;
        }
    
        void OnGUI ()
        {
            LoadTextures();
            DrawRefToLevel();
            _levelData.Gradient = EditorGUILayout.GradientField("Gradient", _levelData.Gradient);
            _levelData.TopOffset = EditorGUILayout.FloatField("Top offset", _levelData.TopOffset);
            DrawFieldSize();
            DrawFieldEditor();
        }

        private void LoadTextures()
        {
            if (_triangleTex == null)
            {
                _triangleTex = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/Graphics/polyg.png", typeof(Texture));
            }
        }

        private void DrawRefToLevel()
        {
            LevelData levelData = _levelData;
            _levelData = (LevelData)EditorGUILayout.ObjectField("Level", _levelData, typeof(LevelData), false);
            if (_levelData != levelData)
            {
                Init(levelData);
            }
        }

        private void DrawFieldEditor()
        {
            float offset = 72;
            if (_levelData.Field.Count > 0 && _levelData.Field[0].Column.Count > 0)
            {
                string[] options = Enum.GetNames(typeof(LevelFieldType));
                for (int i = 0; i < _levelData.Field.Count; i++)
                {
                    for (int j = 0; j < _levelData.Field[i].Column.Count; j++)
                    {
                        var field = _levelData.Field[i].Column[j];
                        Rect rect = new Rect(i * offset, 110 + j * offset, 69, 69);
                        if (field.Exist)
                        {
                            if (field.Type == LevelFieldType.Quad)
                            {
                                EditorGUI.DrawRect(rect, Color.white);
                            }
                            else
                            {
                                float angle = field.Type switch
                                {
                                    LevelFieldType.Quad => 0,
                                    LevelFieldType.LeftBottom => 0,
                                    LevelFieldType.RightTop => 180,
                                    LevelFieldType.RightBottom => 270,
                                    LevelFieldType.LeftTop => 90,
                                    _ => 0,
                                };
                                var pivotPoint = new Vector2(rect.x + rect.width / 2, rect.y + rect.height / 2);
                                GUIUtility.RotateAroundPivot(angle, pivotPoint);
                                GUI.DrawTexture(rect, _triangleTex, ScaleMode.ScaleToFit, true);
                                GUIUtility.RotateAroundPivot(-angle, pivotPoint);
                            }
                            Rect countRect = rect;
                            countRect.height /= 3.5f;
                            countRect.width /= 2f;
                            countRect.x += countRect.width / 2;
                            countRect.y += 3f;
                            
                            field.Count = EditorGUI.IntField(countRect, field.Count);
                            
                            Rect popupRect = rect;
                            popupRect.height /= 3f;
                            popupRect.width *= 0.8f;
                            popupRect.x += 3f;
                            popupRect.y += popupRect.height * 2;

                            field.Type = (LevelFieldType)EditorGUI.Popup(popupRect, (int)field.Type, options);
                        }
                        
                        Rect existRect = rect;
                        existRect.height /= 3f;
                        existRect.width /= 3f;
                        existRect.x += rect.width / 3;
                        existRect.y += rect.height / 3;
                        
                        field.Exist = EditorGUI.Toggle(existRect, field.Exist);
                    } 
                }
            }
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
                    _levelData.Field.Add(new FieldColumn());
                    for (int j = 0; j < _height; j++)
                    {
                        _levelData.Field[i].Column.Add(new Field());
                    }
                }
                EditorUtility.SetDirty(_levelData);
            }

            _prevHeight = _height;
            _prevWidth = _width;
        }
    }
}