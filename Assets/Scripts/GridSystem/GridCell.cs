using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private int _CellX;
    private int _CellY;
    private float _CellSize;
    private Vector3 _CellPosition;

    public int CellX => _CellX;
    public int CellY => _CellY;
    public Vector3 CellPosition => _CellPosition;

    public GridCell(int cellX, int cellY, float size, Vector3 position)
    {
        _CellX = cellX;
        _CellY = cellY;
        _CellSize = size;
        _CellPosition = position;
    }

    
}