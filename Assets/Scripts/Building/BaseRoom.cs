using System.Collections.Generic;
using UnityEngine;

public enum EResourceType
{
    POWER = 0,
    FOOD = 1,
    WATER = 2,
    GRAVITY = 3,
    MISC = 4,
}

public abstract class BaseRoom
{
    protected RoomData _Data;                       // Reference to the room data
    public RoomData Data => _Data;
    protected Timer _ResourceTimer;                 // Timer for generating resources

    // How long it takes for the room to produce resources
    protected float _CurrentResourceTime;  

    protected GridCell[,] _Cells;                               // Reference to the cells that this room is in

    public float CurrentResourceTime 
    {
        get => _CurrentResourceTime;
        set 
        {
            if(_ResourceTimer != null)
                _ResourceTimer.TimerLength = value;

            _CurrentResourceTime = value;
        }
    } 

    protected float _ResourcesPerCycle;                             // Amount of resources that will be produced this cycle
    public float ResourcesPerCycle => _ResourcesPerCycle;

    // === Colonist Settings === //
    protected List<BaseColonist> _Colonist = new List<BaseColonist>();                      // Colonist that are assigned to this room
    public float MaxColonist;                       // Make amount of colonist that can be assigned to the room
    protected int _RoomSize = 1;                            // How many sections this room takes up

    // Flag if the resources are ready for collection
    private bool _IsReadyForCollection = false;
    public bool IsReadyForCollection => _IsReadyForCollection;

    public BaseRoom(RoomData data)
    {
        _Data = data;
        CurrentResourceTime = data.BaseTimePerRoomSize;
        _ResourceTimer = new Timer(_Data.BaseTimePerRoomSize, _ReadyForCollection, false, false);
    }


    public virtual void OnUpdate()
    {
        // Update the resource timer
        if(_ResourceTimer != null)
            _ResourceTimer.OnUpdate();
    }

    protected virtual void _ReadyForCollection()
    {
        _IsReadyForCollection = true;                           // Flag that the resources are ready for collection
        // TODO: Display overlay indicator
    }

    /// <summary>
    /// Based method for collecting resources
    /// </summary>
    public virtual void OnCollectResources()
    {
        if(_ResourceTimer != null)
            if(_Colonist.Count > 0)
                _ResourceTimer.IsActive = true;

        _IsReadyForCollection = false;
    }
    
    /// <summary>
    /// Adds a new colonist to the room
    /// </summary>
    /// <param name="colonist">Colonist to add</param>
    public void AddColonist(BaseColonist colonist)
    {
        // Validate there is room for the colonist
        if (_Colonist.Count > MaxColonist)
            return;
        
        _Colonist.Add(colonist);                    // Add the coloniist
        
        // If resource timer is not active than active it
        if (!_ResourceTimer.IsActive && !_IsReadyForCollection)
            _ResourceTimer.IsActive = true;
        
        _CalculateTimeBasedOnColonist();
    }

    /// <summary>
    /// Calculate how much power is used per second
    /// </summary>
    /// <returns></returns>
    public float PowerPerSecond()
    {
        return _Data._PowerUsagePerSecond * _RoomSize;
    }

    /// <summary>
    /// Recalculates the amount of time for each cycle based on the 
    /// </summary>
    /// <param name="mod"></param>
    protected void CalculateTime(float mod)
    {
        CurrentResourceTime = (_Data.BaseTimePerRoomSize * _RoomSize) * mod;
    }

    protected abstract void _CalculateTimeBasedOnColonist();

    /// <summary>
    /// Assigns the cells to a room
    /// </summary>
    /// <param name="cells">Grid cells this room is over</param>
    public void PlaceRoom(GridCell[,] cells)
    {
        _Cells = cells;

        for(int x = 0; x < _Cells.GetLength(0); ++x)
        {
            for(int y = 0; y < _Cells.GetLength(1); ++y)
            {
                // Spawn the floor tiles
                GameObject spawnedFloor = GameObject.Instantiate(_Data.Floor);
                if(spawnedFloor)
                {
                    spawnedFloor.transform.position = _Cells[x, y].GetCenter();
                }

                if(y == 0)
                {
                    GameObject spawnedWall = GameObject.Instantiate(_Data.Wall);
                    if(spawnedWall != null)
                    {
                        spawnedWall.transform.position = _Cells[x, y].CellPosition;
                        Quaternion quat = Quaternion.identity;
                        quat.eulerAngles = new Vector3(spawnedWall.transform.rotation.x, 90f, spawnedWall.transform.rotation.z);
                        spawnedWall.transform.rotation = quat;
                    }
                } else if( y == _Cells.GetLength(1) - 1)
                {
                    GameObject spawnedWall = GameObject.Instantiate(_Data.Wall);
                    if(spawnedWall != null)
                    {
                        spawnedWall.transform.position = _Cells[x, y].GetCellOffsetY(EOffsetDirection.FORWARDS);
                        Quaternion quat = Quaternion.identity;
                        quat.eulerAngles = new Vector3(spawnedWall.transform.rotation.x, 90f, spawnedWall.transform.rotation.z);
                        spawnedWall.transform.rotation = quat;
                    }
                }

                if(x == 0)
                {
                    GameObject spawnedWall = GameObject.Instantiate(_Data.Wall);
                    if(spawnedWall)
                    {
                        spawnedWall.transform.position = _Cells[x, y].CellPosition;
                    }
                } else if(x == _Cells.GetLength(0) - 1)
                {
                    GameObject spawnedWall = GameObject.Instantiate(_Data.Wall);
                    if(spawnedWall)
                    {
                        spawnedWall.transform.position = _Cells[x, y].GetCellOffsetX(EOffsetDirection.FORWARDS);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adds grid cells to the room
    /// </summary>
    /// <param name="cells">Cells to add to the grid</param>
    public void AddCells(GridCell[,] cells)
    {
        int countX = 0, countY = 0;                     // Define the cell count
        // Create the new grid
        GridCell[,] updatedCells = new GridCell[_Cells.GetLength(0) + cells.GetLength(0), _Cells.GetLength(1) + cells.GetLength(1)];

        // 2D loop to update the cells
        for(int x = 0; x < _Cells.GetLength(0); ++x)
        {
            for(int y = 0; y < _Cells.GetLength(1); ++y)
            {
                updatedCells[countX, countY] = _Cells[x, y];                        // Assign the cells
                countY++;                   // Increment the count
            }
            countX++;                           // Increment count
            countY = 0;                     // Reset count
        }

        // 2D Loop to update with the new cells
        for(int x = 0; x < cells.GetLength(0); ++x)
        {
            for(int y = 0; y < cells.GetLength(1); ++y)
            {
                updatedCells[countX, countY] = cells[x, y];
                countY++;
            }
            countY = 0;
            countX++;
        }
        _Cells = updatedCells;                          // Assign the cells
    }

    public string CalculateAndPrintTimeRemaining()
    {
        if(_ResourceTimer != null)
        {
            int minutes = Mathf.FloorToInt(_ResourceTimer.TimeRemaining / 60);
            int seconds = Mathf.FloorToInt(_ResourceTimer.TimeRemaining % 60);
            if(minutes > 0)
            {
                return $"{minutes}m {seconds}s";
            } else
            {
                return $"{seconds}s";
            }
        }

        return "";
    }

}