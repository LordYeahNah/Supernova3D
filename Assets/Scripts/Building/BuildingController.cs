using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour 
{
    public bool IsInBuildMode = false;
    [SerializeField] private GridController _Grid;
    [SerializeField] private LayerMask _GroundLayer;


    private bool _IsDragging = false;
    private GridCell _StartCell;
    private GridCell[,] _SelectedCells;

    private Camera _Cam;

    private void Awake()
    {
        if(!_Grid)
            Debug.LogError("Failed to initialize grid");

        if(!_Cam)
            _Cam = Camera.main;
    }

    private void Update()
    {
        if(!IsInBuildMode)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            _StartCell = _CastToGridCell();
            _IsDragging = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            _StartCell = null;
            _IsDragging = false;
            _DeselectCells();
        }

        if(_IsDragging)
            _CalculateSelectedCells();
        
    }

    private GridCell _CastToGridCell()
    {
        if(!_Cam)
            return null;

        Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000, _GroundLayer))
        {
            if(_Grid != null)
            {
                return _Grid.GetCell(hit.point);
            }
        }

        return null;
    }

    private void _CalculateSelectedCells()
    {
        if(!_IsDragging)
            return;

        _DeselectCells();

        GridCell currentCell = _CastToGridCell();                       // Get reference to the current cell
        if(currentCell != null)
        {
            int cellsX = currentCell.CellX - (_StartCell.CellX);
            int cellsY = currentCell.CellY - (_StartCell.CellY);

            int xCellAmount = Mathf.Abs(cellsX);
            int yCellAmount = Mathf.Abs(cellsY);

            _SelectedCells = new GridCell[xCellAmount + 1, yCellAmount + 1];
            
            int cellCountX = 0, cellCountY = 0;
            for(int x = _StartCell.CellX; x <= currentCell.CellX; ++x)
            {
                for(int y = _StartCell.CellY; y <= currentCell.CellY; ++y)
                {
                    GridCell cell = _Grid.GetCell(x, y);
                    if(cell != null)
                    {
                        cell.IsSelected = true;
                    }

                    cellCountY += 1;
                }
                cellCountX += 1;
                cellCountY = 0;
            }
        }

    }

    private void _DeselectCells()
    {
        if(_SelectedCells != null)
        {
            for(int x = 0; x < _SelectedCells.GetLength(0); ++x)
            {
                for(int y = 0; y < _SelectedCells.GetLength(1); ++y)
                {
                    GridCell cell = _SelectedCells[x, y];
                    if(cell != null)
                        cell.IsSelected = false;
                }
            }
        }
    }
}