using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Layer Mask")]
    [SerializeField] private LayerMask _RoomLayerMask;

    [Header("Components")]
    [SerializeField] private Camera _Cam;
    [SerializeField] private GridController _Grid;                      // Store reference to the current grid controller
    [SerializeField] private BuildingController _Building;                      // Store reference to the building controller
    [SerializeField] private RoomInfoHUD _RoomInfoUI;                    // Store reference to the room UI
    private bool _RoomInfoActive = false;
    private void Awake()
    {
        // Get reference to the camera
        if(!_Cam)
            _Cam = Camera.main;
    }

    private void Update()
    {
        if(!_Cam)
            return;

        if(!_Building.IsInBuildMode)
        {
            if(Input.GetMouseButtonDown(0))
            {
                CastToRoom();
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(_RoomInfoActive)
                {
                    _RoomInfoUI.gameObject.SetActive(false);
                    _RoomInfoActive = false;
                }
            }
        }
    }

    private bool CastToRoom()
    {
        // Verifiy reference to the grid
        if(!_Grid)
            return false;

        Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, _RoomLayerMask))
        {
            GridCell cell = _Grid.GetCell(hit.point);
            if(cell != null && cell.HasAssignedRoom)
            {
                DisplayRoom(cell._AssignedRoom);
                return true;
            }
        }

        return false;
    }

    private void DisplayRoom(BaseRoom room)
    {
        if(_RoomInfoUI)
        {
            _RoomInfoUI.gameObject.SetActive(true);
            _RoomInfoUI.Setup(room);
            _RoomInfoActive = true;
        }
    }
}
