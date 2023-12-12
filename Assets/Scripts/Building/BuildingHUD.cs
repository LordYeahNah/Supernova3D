using JetBrains.Annotations;
using UnityEngine;

public class BuildingHUD : MonoBehaviour
{
    [SerializeField] private BuildingController _Controller;
    [SerializeField] private GameObject _ConfirmPlacementUI;                            // Reference to the buttons for confirming placement
    [SerializeField] private GameObject _BuildingMenu;
    public void _OnSelectBuilding(string roomName)
    {
        RoomData room = RoomDatabase.Instance.GetRoom(roomName);
        if(room != null)
        {
            if(_Controller != null)
                _Controller.SetPlacingRoom(room);

            ToggleRoomMenu(false);
            ToggleConfirmPlacement(true);
        } else 
        {
            Debug.LogError("#BuildingHUD::_OnSelectBuilding --> Failed to retrieve room");
        }
    }  

    /// <summary>
    /// Shows and hides the confirm placement menu
    /// </summary>
    /// <param name="visible">If the menu should be visible</param>
    public void ToggleConfirmPlacement(bool visible)
    {
        if(_ConfirmPlacementUI)
            _ConfirmPlacementUI.SetActive(visible);
    }  

    /// <summary>
    /// Toggles the select room menu
    /// </summary>
    /// <param name="visible"></param>
    public void ToggleRoomMenu(bool visible)
    {
        if(_BuildingMenu)
            _BuildingMenu.SetActive(visible);
    }

    public void _OnConfirmPlacement(bool confirm)
    {
        ToggleConfirmPlacement(false);
        ToggleRoomMenu(true);
        
        if(_Controller)
            _Controller.OnConfirmPlacement(confirm);
        
    }
}