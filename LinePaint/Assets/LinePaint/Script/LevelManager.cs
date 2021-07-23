using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CameraZoom gameCamera;
        [SerializeField] private CameraZoom solutionCamera;
        [SerializeField] private Cell blockPrefab;
        [SerializeField] private BrushController brushPrefab;
        [SerializeField] private LinePaintScript linePaintPrefab;
        [SerializeField] private float cellSize;
        [SerializeField] private List<LevelScriptableData> levelScriptables;
        [SerializeField] private Vector3 gridOriginPos; 
        private int width;
        private int height;
        private Cell[,] cellArray;
        private Grid grid;
        private SwipeController swipeController;
        private BrushController currentBrush;
        private List<ConnectionLine> inProgress = new List<ConnectionLine>();
        private List<LinePaintScript> connectedLinePaint = new List<LinePaintScript>();
        private void Start()
        {
            swipeController = new SwipeController();
            swipeController.SetLevelManager(this);
            
            width = levelScriptables[GameManager.currentLevel].width;
            height = levelScriptables[GameManager.currentLevel].height;
            
            grid = new Grid();
            CompleteBoard();

            grid.Initialize(width, height, cellSize, Vector3.zero);
            cellArray = new Cell[width, height];

            CreateGrid(Vector3.zero);

            currentBrush = Instantiate(brushPrefab, grid.GetCellWorldPosition(levelScriptables[GameManager.currentLevel].brushStartCoords.x, levelScriptables[GameManager.currentLevel].brushStartCoords.y), Quaternion.identity);
            currentBrush.currentCoords = new Vector2Int(0, 0);
            
            gameCamera.ZoomPerspectiveCamera(width, height);
        }
        private void Update()
        {
            if(swipeController != null && GameManager.gameStatus == GameStatus.Playing)
            {
                swipeController.OnUpdate();
            }
        }
        private Cell CreateCell(int x, int y, Vector3 originPos)
        {
            Cell cell = Instantiate(blockPrefab);
            cell.cellCoords = new Vector2Int(x,y);
            cell.transform.localScale = new Vector3(cellSize, 0.25f, cellSize);
            cell.transform.position = originPos + grid.GetCellWorldPosition(x, y);
            
            return cell;
        }
        public void MoveBrush(Swipe direction)
        {
            //Debug.Log(direction);
            Vector2Int newCoords = grid.GetCellXZBySwipe(currentBrush.currentCoords.x, currentBrush.currentCoords.y, direction);

            if(newCoords != new Vector2Int(-1, -1))
            {
                Vector3 finalPos = grid.GetCellWorldPosition(newCoords.x, newCoords.y);

                if(ConnectionAlreadyDone(currentBrush.currentCoords, newCoords, true) == false)
                {
                    inProgress.Add(new ConnectionLine(currentBrush.currentCoords, newCoords));
                    
                    cellArray[currentBrush.currentCoords.x, currentBrush.currentCoords.y].CellCenterPaint.gameObject.SetActive(true);

                    LinePaintScript linePaint = Instantiate(linePaintPrefab, new Vector3(0, 0.2f, 0), Quaternion.identity);
                    linePaintPrefab.SetRendererPosition(currentBrush.transform.position + new Vector3(0, 0.2f, 0),
                    finalPos + new Vector3(0, 0.2f, 0));
                    linePaint.SetConnectedCoords(currentBrush.currentCoords, newCoords);
                    connectedLinePaint.Add(linePaint);
                }
                else
                {
                    RemoveConnectLinePaint(currentBrush.currentCoords, newCoords);
                }

                if(levelScriptables[GameManager.currentLevel].completePattern.Count <= inProgress.Count)
                {
                    if(IsLevelComplete())
                    {
                        GameManager.gameStatus = GameStatus.Complete;
                        Debug.Log("LevelCompleted!");
                    }
                }

                currentBrush.transform.position = finalPos;
                currentBrush.currentCoords = newCoords;
            }
        }
        private bool ConnectionAlreadyDone(Vector2Int startCoord, Vector2Int endCoord, bool removeConnectLine)
        {
            bool connected = false;

            for (int i = 0; i < inProgress.Count; i++)
            {
                if(inProgress[i].StartCoords == startCoord && inProgress[i].EndCoords == endCoord ||
                    inProgress[i].StartCoords == endCoord && inProgress[i].EndCoords == startCoord)
                {
                    if(removeConnectLine)
                    {
                        inProgress.RemoveAt(i);
                    }

                    connected = true;
                    break;
                }
            }
            return connected;
        }
        private void RemoveConnectLinePaint(Vector2Int startCoord, Vector2Int endCoord)
        {
            for (int i = 0; i < connectedLinePaint.Count; i++)
            {
                if(connectedLinePaint[i].StartCoords == startCoord && connectedLinePaint[i].EndCoords == endCoord ||
                    connectedLinePaint[i].StartCoords == endCoord && connectedLinePaint[i].EndCoords == startCoord)
                {
                    LinePaintScript line = connectedLinePaint[i];
                    connectedLinePaint.RemoveAt(i);
                    Destroy(line.gameObject);

                    cellArray[endCoord.x, endCoord.y].CellCenterPaint.gameObject.SetActive(false);
                }
            }
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
        private bool IsLevelComplete()
        {
            if(levelScriptables[GameManager.currentLevel].completePattern.Count != inProgress.Count)
            {
                return false;
            }

            for(int i = 0; i < levelScriptables[GameManager.currentLevel].completePattern.Count; i++)
            {
                if(!ConnectionAlreadyDone(levelScriptables[GameManager.currentLevel].completePattern[i].StartCoords,
                    levelScriptables[GameManager.currentLevel].completePattern[i].EndCoords, false))
                    {
                        return false;
                    }
            }
            return true;
        }
        private void CompleteBoard()
        {
            grid.Initialize(width, height, cellSize, gridOriginPos);

            Vector3 offset = new Vector3((levelScriptables[GameManager.currentLevel].width - cellSize) / 2, 0, (levelScriptables[GameManager.currentLevel].height - cellSize) / 2);
            solutionCamera.transform.position += offset;

            solutionCamera.ZoomOrthographicSizeCamera(levelScriptables[GameManager.currentLevel].width, levelScriptables[GameManager.currentLevel].height);

            for (int i = 0; i < levelScriptables[GameManager.currentLevel].completePattern.Count; i++)
            {
                Vector3 startPos = grid.GetCellWorldPosition(levelScriptables[GameManager.currentLevel].completePattern[i].StartCoords.x,
                    levelScriptables[GameManager.currentLevel].completePattern[i].StartCoords.y);

                Vector3 endPos = grid.GetCellWorldPosition(levelScriptables[GameManager.currentLevel].completePattern[i].EndCoords.x,
                    levelScriptables[GameManager.currentLevel].completePattern[i].EndCoords.y);

                LinePaintScript linePaint = Instantiate(linePaintPrefab, new Vector3(0, 0.2f, 0), Quaternion.identity);
                linePaint.SetRendererPosition(startPos, endPos);
            }
        } 
    }
}
