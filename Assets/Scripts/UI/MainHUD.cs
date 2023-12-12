using TMPro;
using UnityEngine;

public class MainHUD : MonoBehaviour 
{
    [SerializeField] private BuildingController _BuildingController;
    [SerializeField] private TMP_Text _StateButtonText;

    [SerializeField] private GameObject _BuildingHUD;

    private void Awake()
    {
        if(!_BuildingController)
            Debug.LogError("#MainHUD::Awake --> Failed to load building controller");

        if(!_BuildingHUD)
            Debug.LogError("#MainHUD::Awake --> Failed to load building hud");
    }    

    public void TogglePlayState()
    {
        if(!_BuildingController)
            return;

        _BuildingController.IsInBuildMode = !_BuildingController.IsInBuildMode;
        if(_BuildingController.IsInBuildMode)
        {
            _StateButtonText.text = "Play";
            if(_BuildingHUD)
                _BuildingHUD.SetActive(true);
        } else 
        {
            _StateButtonText.text = "Build";
            if(_BuildingHUD)
                _BuildingHUD.SetActive(false);
        }
    }
}