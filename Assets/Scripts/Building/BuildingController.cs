using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BuildingController : MonoBehaviour 
{
    public bool IsInBuildMode = false;                              // If we are currently in build mode

    // === Building Placement === //
    private bool IsPlacingRoom = false;                                 // If we have selected a building and are placing it
    private RoomData _SelectedRoom;                                 // Room that has been selected to build

    // === Door Placement === //
    private bool IsPlacingDoor = false;
    private GameObject _MouseOverWall = null;                               // reference to the wall the mouse is currently over
    private GameObject _SpawnedDoor = null;                             // Reference to the spawned door
    [SerializeField] private LayerMask _WallLayer;

    
    // === Misc === //
    [SerializeField] private GridController _Grid;                          // Reference to the grid controller
    [SerializeField] private LayerMask _GroundLayer;                    // Layer mask for placing only on ground
    
    [SerializeField] private BuildingHUD _BuildingHUD;                      // Reference to the building HUD
    


    private bool _IsDragging = false;                                   // If we are currently dragging
    private GridCell _StartCell;                                        // Reference to the cell that has started the dragging
    private GridCell[,] _SelectedCells;                                 // Reference to the cells that have been selected

    private Camera _Cam;                                    // Store reference to the camera

    private void Awake()
    {
        // Validate grid
        if(!_Grid)
            Debug.LogError("Failed to initialize grid");

        // Get reference to the camera
        if(!_Cam)
            _Cam = Camera.main;
    }

    private void Update()
    {
        // Only update when in build mode
        if(!IsInBuildMode)
            return; 

        // Begin dragging
        if(Input.GetMouseButtonDown(0))
        {
            if(_SelectedRoom != null)
            {
                if(IsPlacingRoom)
                {
                    _DeselectCells();
                    _StartCell = _CastToGridCell();
                    _IsDragging = true;
                }
            } else 
            {
                if(IsPlacingDoor)
                    PlaceDoor();
            }
        }

        if(Input.GetMouseButtonUp(0) && _SelectedRoom != null && IsPlacingRoom)
        {       
            if(_SelectedCells != null && _SelectedCells.GetLength(0) > 0 && _SelectedCells.GetLength(1) > 0)
            {
                if(_BuildingHUD)
                    _BuildingHUD.ToggleConfirmPlacement(true);

                _IsDragging = false;
                _StartCell = null;
            }
        }

        if(IsPlacingDoor)
            _CastDoorToWall();


        if(_IsDragging)
            _CalculateSelectedCells();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_BuildingHUD)
                _BuildingHUD.ToggleConfirmPlacement(false);

            _DeselectAll();
        }


        
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
        if(!_IsDragging || _StartCell == null)
            return;

        _DeselectCells();

        GridCell currentCell = _CastToGridCell();                       // Get reference to the current cell
        if(currentCell != null)
        {
            int cellsX = currentCell.CellX - (_StartCell.CellX);
            int cellsY = currentCell.CellY - (_StartCell.CellY);

            int xCellAmount = Mathf.Abs(cellsX);
            int yCellAmount = Mathf.Abs(cellsY);

            _SelectedCells = new GridCell[xCellAmount, yCellAmount];
            
            int cellCountX = 0, cellCountY = 0;

            int xIncrement = (cellsX > 0) ? 1 : -1;
            for(int x = _StartCell.CellX; x != currentCell.CellX; x += xIncrement)
            {
                int yIncrement = (cellsY > 0) ? 1 : -1;
                for(int y = _StartCell.CellY; y != currentCell.CellY; y += yIncrement)
                {
                    GridCell cell = _Grid.GetCell(x, y);
                    if(cell != null)
                    {
                        cell.IsSelected = true;
                        _SelectedCells[cellCountX, cellCountY] = cell;
                    }

                    cellCountY++;
                }

                cellCountX++;
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

    public void OnConfirmPlacement(bool placed)
    {
        if(placed)
        {
            BaseRoom placingRoom = CreateRoom(_SelectedRoom);

            if(_SelectedCells.GetLength(0) < placingRoom.Data.MinCellsX && _SelectedCells.GetLength(1) < placingRoom.Data.MinCellsY)
            {
                Debug.Log("#BuildingController::OnConfirmPlacement --> Invalid Size");
                _DeselectCells();
                return;
            }

            bool cancelPlacement = false;
            GridCell[,] cells = new GridCell[_SelectedCells.GetLength(0), _SelectedCells.GetLength(1)];

            for(int x = 0; x < cells.GetLength(0); ++x)
            {
                for(int y = 0; y < cells.GetLength(1); ++y)
                {
                    GridCell cellRef = _SelectedCells[x, y];
                    if(cellRef != null)
                    {
                        if(cellRef.HasAssignedRoom)
                        {
                            Debug.Log("#BuildingController::OnConfirmPlacement --> Cell is in use");
                            // TODO: Display error placing message
                            cancelPlacement = true;
                            _DeselectCells();
                            break;
                        } else 
                        {
                            cells[x, y] = cellRef;
                        }

                        if(cancelPlacement)
                        {
                            break;
                        }
                    } else 
                    {
                        Debug.Log("#BuildingController::OnConfirmPlacement --> Cell is null");
                        cancelPlacement = true;
                        _DeselectCells();
                        break;
                    }
                }   
            }

            if(!cancelPlacement)
            {
                for(int x = 0; x < cells.GetLength(0); ++x)
                {
                    for(int y = 0; y < cells.GetLength(1); ++y)
                    {
                        cells[x, y]._AssignedRoom = placingRoom;
                    }
                }
                
                placingRoom.PlaceRoom(cells);

                if(!_SpawnedDoor)
                    _SpawnedDoor = GameObject.Instantiate(_SelectedRoom.Door);

                IsPlacingRoom = false;
                IsPlacingDoor = true;
                _DeselectAll();
            } 
        } else 
        {
            _DeselectAll();
            IsPlacingRoom = false;
        }
    }

    private BaseRoom CreateRoom(RoomData data)
    {
        switch(data.RoomName)
        {
            case "Power Generator":
                return new PowerGenerator(data);
            case "Food Synthesis":
                return new FoodSynthesis(data);
        }

        return null;
    }

    public void SetPlacingRoom(RoomData room)
    {
        _SelectedRoom = room;
        if(room != null)
        {
            StartCoroutine(_EnablePlacingRoom(true));
        } else 
        {
            IsPlacingRoom = false;
        }
    }

    private IEnumerator _EnablePlacingRoom(bool setPlacing)
    {
        yield return new WaitForSeconds(0.5f);
        IsPlacingRoom = setPlacing;
    }

    private void _DeselectAll()
    {
        _StartCell = null;
        _IsDragging = false;
        SetPlacingRoom(null);
        _SelectedCells = null;
    }

    private void _CastDoorToWall()
    {
        if(!_Cam)
            return;

        // Reset the wall
        if(_MouseOverWall)
            _MouseOverWall.SetActive(true);

        Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);                       // Create the ray for the raycast
        
        // Perform the raycast
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, _WallLayer))
        {
            // Check hit is valid
            if(hit.collider)
            {
                _MouseOverWall = hit.collider.gameObject;                       // Set the wall that the mouse is over

                // Disable the wall the mouse is over
                if(_MouseOverWall)
                    _MouseOverWall.SetActive(false);

                if(_SpawnedDoor)
                {
                    _SpawnedDoor.transform.position = _MouseOverWall.transform.position;
                    _SpawnedDoor.transform.rotation = _MouseOverWall.transform.rotation;
                } else 
                {
                    Debug.LogError("#BuildingController::_CastToDoor -- > Door has not been spawned");
                }
            }
        }
    }

    private void PlaceDoor()
    {
        if(_SpawnedDoor != null)
        {
            Destroy(_MouseOverWall);
            _SpawnedDoor = null;
            IsPlacingDoor = false;
        }
    }
}