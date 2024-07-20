using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    [CreateAssetMenu(menuName = "Create new level", fileName = "Level")]
    public class LevelData : ScriptableObject
    {
        public Gradient Gradient;
        public List<FieldColumn> Field = new();
        public float TopOffset;
    }

    [Serializable]
    public class FieldColumn
    {
        public List<Field> Column = new();
    }

    [Serializable]
    public class Field
    {
        public LevelFieldType Type;
        public int Count = 1;
        public bool Exist = true;
    }

    public enum LevelFieldType
    {
        Quad,
        LeftBottom,
        RightTop,
        RightBottom,
        LeftTop
    }
}
