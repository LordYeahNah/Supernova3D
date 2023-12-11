using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // === Grid Settings === //
    [Header("Grid Settings")]
    [SerializeField] private int _CellsX;
    [SerializeField] private int _CellsY;
    [SerializeField] private float _CellSize;
    [SerializeField] private bool _DrawGrid = true;

    public int CellsX => _CellsX;
    public int CellsY => _CellsY;
    public float CellSize => _CellSize;

    private GridCell[,] _Grid;                      // Reference to the grid

    private void Awake()
    {
        _CreateGrid();
    }

    /// <summary>
    /// Handles creating a new grid
    /// </summary>
    private void _CreateGrid()
    {
        _Grid = new GridCell[_CellsX, _CellsY];                     // Initialize the array
        // Determine the start position
        Vector3 startPosition = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        // Define the current location of the grid
        Vector3 currentPosition = new Vector3(startPosition.x, 0f, startPosition.z);

        for(int y = 0; y < _Grid.GetLength(1); ++y)
        {
            for(int x = 0; x < _Grid.GetLength(0); ++x)
            {
                _Grid[x, y] = new GridCell(x, y, _CellSize, currentPosition);                   // Create the grid cell

                // Update the current position of the cell
                currentPosition.x += _CellSize;
            }

            // Update the current position of the cell
            currentPosition.x = startPosition.x;
            currentPosition.z += _CellSize;
        }
    }

    private void OnDrawGizmos() 
    {
        if(!_DrawGrid)
            return;

        if(_Grid != null)
        {
            Gizmos.color = Color.red;
            for(int y = 0; y < _Grid.GetLength(1); ++y)
            {
                for(int x = 0; x < _Grid.GetLength(0); ++x)
                {
                    GridCell cell = _Grid[x, y];
                    if(cell != null)
                    {
                        Vector3 startPos = cell.CellPosition;
                        Vector3 endX = new Vector3(startPos.x + _CellSize, 0f, startPos.z);
                        Vector3 endY = new Vector3(startPos.x, 0f, startPos.z + _CellSize);

                        Gizmos.DrawLine(startPos, endX);
                        Gizmos.DrawLine(startPos, endY);
                    }
                }
            }
        }
    }
}
