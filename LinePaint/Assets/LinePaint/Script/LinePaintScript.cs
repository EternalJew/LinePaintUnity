using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    public class LinePaintScript : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        private Vector2Int _startCoords;
        private Vector2Int _endCoords;
        public Vector2Int StartCoords { get => _startCoords; }
        public Vector2Int EndCoords { get => _endCoords; }
        public void SetConnectedCoords(Vector2Int startCoords, Vector2Int endCoords)
        {
            _startCoords = startCoords;
            _endCoords = endCoords; 
        }
        public void SetRendererPosition(Vector3 startPos, Vector3 endPos)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
}
