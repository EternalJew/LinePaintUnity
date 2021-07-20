using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private MeshRenderer cellCenterPaint;

        [HideInInspector] public Vector2Int cellCoords;

        public Vector2Int CellCoords { get => cellCoords; set => cellCoords = value; }
        public MeshRenderer CellCenterPaint { get => cellCenterPaint; }
    }
}
