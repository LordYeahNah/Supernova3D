using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Layer Mask")]
    [SerializeField] private LayerMask _GeneralMask;
    [SerializeField] private LayerMask _CharacterDragMask;

    [Header("Hold Settings")]
    [SerializeField, Tooltip("How long to hold character before dragging"), Range(0, 1)] 
    private float _CharacterHoldTime = 0.5f;
    private bool _IsMouseDown = false;
    private bool _IsDragging = false;
    private GameObject _SelectedCharacter;                          // Reference to the selected character
    private Vector3 _SelectedCharacterInitialPosition;                      // Reference to where the character was prior to dragging

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
                _PerformCast();
                _IsMouseDown = true;
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(_RoomInfoActive)
                {
                    _RoomInfoUI.gameObject.SetActive(false);
                    _RoomInfoActive = false;
                }
            }

            if(_IsDragging)
            {
                _DragCharacter();
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(_IsDragging && _SelectedCharacter)
            {
                _DetectAddCharacterToRoom();
            }

            _IsMouseDown = false;
        }


    }

    private bool _PerformCast()
    {
        // Verifiy reference to the grid
        if(!_Grid)
            return false;

        Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, _GeneralMask))
        {
            if(hit.collider.CompareTag("Colonist"))
            {
                _SelectedCharacter = hit.collider.gameObject;
                StartCoroutine(_DetermineIfHolding());
            } else 
            {
                GridCell cell = _Grid.GetCell(hit.point);
                if(cell != null && cell.HasAssignedRoom)
                {
                    DisplayRoom(cell._AssignedRoom);
                    return true;
                }
            }
        }

        return false;
    }

    private void _DragCharacter()
    {
        if(!_SelectedCharacter)
        {
            _IsDragging = false;
        }
        Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, _CharacterDragMask))
        {
            _SelectedCharacter.transform.position = hit.point;
        }
    }

    private IEnumerator _DetermineIfHolding()
    {
        yield return new WaitForSeconds(_CharacterHoldTime);
        if(_IsMouseDown)
        {
            _IsDragging = true;
            _SelectedCharacter.GetComponent<ColonistController>()?.StopMovement();
            _SelectedCharacterInitialPosition = _SelectedCharacter.transform.position;
        }
        else
        {
            _SelectedCharacter = null;
        }
    }

    private void _DetectAddCharacterToRoom()
    {
        Ray ray = _Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, _CharacterDragMask))
        {
            GridCell cell = _Grid.GetCell(hit.point);
            if(cell != null && cell.HasAssignedRoom)
            {
                ColonistController controller = _SelectedCharacter.GetComponent<ColonistController>();
                if(controller != null)
                {
                    BaseColonist colonist = controller.Colonist;
                    if(colonist != null)
                    {
                        if(cell._AssignedRoom.AddColonist(colonist))
                        {
                            colonist.SetLocation(cell.CellPosition);
                        }
                    }
                }
            }
        }

        _SelectedCharacter.transform.position = _SelectedCharacterInitialPosition;
        _SelectedCharacter = null;
        _IsDragging = false;
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
