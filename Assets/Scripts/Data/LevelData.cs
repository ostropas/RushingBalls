using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create new level", fileName = "Level")]
    public class LevelData : ScriptableObject
    {
        public Gradient Gradient;
        public List<List<Field>> Field = new();
    }

    [Serializable]
    public class Field
    {
        public LevelFieldType Type;
        public int Count;
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
