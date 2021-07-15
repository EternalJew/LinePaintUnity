using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CameraZoom gameCamera;
        [SerializeField] private Cell blockPrefab;
        [SerializeField] private BrushController brush;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;
        private Cell[,] cellArray;
        private Grid grid;
        private SwipeController swipeController;
        private BrushController currentBrush;
        private void Start()
        {
            swipeController = new SwipeController();
            swipeController.SetLevelManager(this);
            grid = new Grid();
            grid.Initialize(width, height, cellSize);
            cellArray = new Cell[width, height];

            CreateGrid(Vector3.zero);

            currentBrush = Instantiate(brush, grid.GetCellWorldPosition(0, 0), Quaternion.identity);
            currentBrush.currentCoords = new Vector2Int(0, 0);
            
            gameCamera.ZoomPerspectiveCamera(width, height);
        }
        private void CreateGrid(Vector3 originPos)
        {
            for (int x = 0; x < grid.GridArray.GetLength(0); x++)
            {
                for(int y = 0; y < grid.GridArray.GetLength(1); y++)
                {
                    cellArray[x,y] = CreateCell(x, y, originPos);
                }
            }
        }
        private Cell CreateCell(int x, int y, Vector3 originPos)
        {
            Cell cell = Instantiate(blockPrefab);
            cell.cellCoords = new Vector2Int(x,y);
            cell.transform.localScale = new Vector3(cellSize, 0.25f, cellSize);
            cell.transform.position = originPos + grid.GetCellWorldPosition(x,y);
            
            return cell;
        }
        public void MoveBrush(Swipe direction)
        {
            //Debug.Log(direction);
            Vector2Int newCoords = grid.GetCellXZBySwipe(currentBrush.currentCoords.x, currentBrush.currentCoords.y, direction);

            if(newCoords != new Vector2Int(-1, -1))
            {
                Vector3 finalPos = grid.GetCellWorldPosition(newCoords.x, newCoords.y);

                currentBrush.transform.position = finalPos;
                currentBrush.currentCoords = newCoords;
            }
        }
        private void Update()
        {
            if(swipeController != null)
            {
                swipeController.OnUpdate();
            }
        }
    }
}
