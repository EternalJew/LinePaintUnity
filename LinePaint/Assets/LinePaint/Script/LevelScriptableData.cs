using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Create LevelDataScriptable", order = 1)]
    public class LevelScriptableData : ScriptableObject
    {
        public int width, height;
        public Vector2Int brushStartCoords;
        public List<ConnectionLine> completePattern = new List<ConnectionLine>();
    }
}
