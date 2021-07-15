using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
  public class Grid 
  {
    private int _width;
    private int _height;
    private float _cellSize;

    private int[,] gridArray;

    public int[,] GridArray { get { return gridArray; } }

    public void Initialize(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        gridArray = new int[width, height];
    }
    public Vector3 GetCellWorldPosition(int x, int y)
    {
        return new Vector3(Mathf.FloorToInt(_cellSize * x), 0, Mathf.FloorToInt(_cellSize * y));
    }

    public Vector2Int GetCellXZBySwipe(int x, int z, Swipe swipe)
    {
        Vector2Int xz = new Vector2Int(-1, -1);

        switch (swipe)
        {
                case Swipe.Top:
                    if (z < (_height - 1))
                    {
                        xz = new Vector2Int(x, z + 1);
                    }
                    break;
                case Swipe.Bottom:
                    if (z > 0)
                    {
                        xz = new Vector2Int(x, z - 1);
                    }
                    break;
                case Swipe.Left:
                    if (x > 0)
                    {
                        xz = new Vector2Int(x - 1, z);
                    }
                    break;
                case Swipe.TopLeft:
                    if (x > 0 && z < (_height - 1))
                    {
                        xz = new Vector2Int(x - 1, z + 1);
                    }
                    break;
                case Swipe.BottomLeft:
                    if (x > 0 && z > 0)
                    {
                        xz = new Vector2Int(x - 1, z - 1);
                    }
                    break;
                case Swipe.Right:
                    if (x < (_width - 1))
                    {
                        xz = new Vector2Int(x + 1, z);
                    }
                    break;
                case Swipe.TopRight:
                    if (x < (_width - 1) && z < (_height - 1))
                    {
                        xz = new Vector2Int(x + 1, z + 1);
                    }
                    break;
                case Swipe.BottomRight:
                    if (x < (_width - 1) && z > 0)
                    {
                        xz = new Vector2Int(x + 1, z - 1);
                    }
                    break;   
        }
        return xz;
    }
  }
}
