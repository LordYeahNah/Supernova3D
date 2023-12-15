using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class RoomInfoHUD : MonoBehaviour
{
    private BaseRoom _ViewingRoom;                              // Reference to the room we are currently viewing

    // === Components === //
    [Header("Components")]
    [SerializeField] private GameObject _ReadyButton;
    [SerializeField] private TMP_Text _ReadyInText;
    [SerializeField] private TMP_Text _RoomName;
    [SerializeField] private TMP_Text _SettlersInRoom;

    public void Setup(BaseRoom room)
    {
        _ViewingRoom = room;
        if(_ViewingRoom != null)
        {
            if(_RoomName)
                _RoomName.text = _ViewingRoom.Data.RoomName;
        }
    }

    private void Update()
    {
        UpdateCollectionText();
    }

    /// <summary>
    /// Handles displaying the ready text and the ready button
    /// </summary>
    private void UpdateCollectionText()
    {
        if(_ViewingRoom == null)
            return;

        if(_ReadyButton && _ReadyInText)
        {
            if(_ViewingRoom.IsReadyForCollection)
            {
                _ReadyButton.SetActive(true);
                _ReadyInText.gameObject.SetActive(false);
            } else 
            {
                _ReadyButton.SetActive(false);
                _ReadyInText.gameObject.SetActive(true);
                _ReadyInText.text = $"Ready In: {_ViewingRoom.CalculateAndPrintTimeRemaining()}";
            }
        }
    }

    public void _OnCollectResources()
    {
        if(_ViewingRoom != null)
        {
            _ViewingRoom.OnCollectResources();
        }
    }

}