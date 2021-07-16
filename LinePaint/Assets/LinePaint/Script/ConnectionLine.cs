using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    [System.Serializable]
    public class ConnectionLine 
    {
        private Vector2Int _startCoords;
        private Vector2Int _endCoords;
        public ConnectionLine(Vector2Int startCoords, Vector2Int endCoords)
        {
            _startCoords = startCoords;
            _endCoords = endCoords;
        }
        public Vector2Int StartCoords { get => _startCoords;}
        public Vector2Int EndCoords { get => _endCoords;}
    }
}
