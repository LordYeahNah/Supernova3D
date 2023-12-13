using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EOffsetDirection
{
    FORWARDS,
    BACKWARDS
}

public class GridCell
{
    private int _CellX;
    private int _CellY;
    private float _CellSize;
    private Vector3 _CellPosition;

    public int CellX => _CellX;
    public int CellY => _CellY;
    public Vector3 CellPosition => _CellPosition;

    // === Cell Flags === //
    public bool IsSelected = false;                             // If the cell has been selected
    public BaseRoom _AssignedRoom;                             // Room that has been built here
    public bool HasAssignedRoom => _AssignedRoom == null ? false : true;

    public GridCell(int cellX, int cellY, float size, Vector3 position)
    {
        _CellX = cellX;
        _CellY = cellY;
        _CellSize = size;
        _CellPosition = position;
    }

    public Vector3 GetCenter()
    {
        return new Vector3
        {
            x = _CellPosition.x + (_CellSize / 2),
            y = 0f,
            z = _CellPosition.z + (_CellSize / 2)
        };
    }

    /// <summary>
    /// Returns the center, offset from this cell
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterOffset()
    {
        return new Vector3
        {
            x = _CellPosition.x - (_CellSize / 2),
            y = 0f,
            z = _CellPosition.z - (_CellSize / 2)
        };
    }

    public Vector3 GetCellOffsetX(EOffsetDirection direction, int cells = 1)
    {
        if(direction == EOffsetDirection.FORWARDS)
        {
            return new Vector3
            {
                x = _CellPosition.x + _CellSize * cells,
                y = 0,
                z = _CellPosition.z
            };
        } else 
        {
            return new Vector3
            {
                x = _CellPosition.x - _CellSize * cells,
                y = 0,
                z = _CellPosition.z
            };
        }
    }

    public Vector3 GetCellOffsetY(EOffsetDirection direction, int cells = 1)
    {
        if(direction == EOffsetDirection.FORWARDS)
        {
            return new Vector3
            {
                x = _CellPosition.x,
                y = 0f,
                z = _CellPosition.z + _CellSize * cells
            };
        } else 
        {
            return new Vector3
            {
                x = _CellPosition.x,
                y = 0f,
                z = _CellPosition.z - _CellSize * cells
            };
        }
    }

    public Vector3 MultiOffset(int cellsX, int cellsY)
    {
        return new Vector3
        {
            x = _CellPosition.x + (_CellSize * cellsX),
            y = 0f,
            z = _CellPosition.z + (_CellSize * cellsY)
        };
    }

    public override string ToString() => $"X: {_CellX}, Y: {_CellY}";
}