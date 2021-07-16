using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    public class LinePaintScript : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        private Vector2Int _startCoords, _endCoords;
        public Vector2Int StartCoords { get => _startCoords;}
        public Vector2Int EndCoords { get => _endCoords;}
        public void SetConnectedCoords(Vector2Int startCoords, Vector2Int endCoords)
        {
            _startCoords = startCoords;
            _endCoords = endCoords; 
        }
        public void SetRenderePosition(Vector2 startPos, Vector2 endPos)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
}
