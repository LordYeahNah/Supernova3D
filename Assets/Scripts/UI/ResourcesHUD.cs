using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _FoodLabel;
    [SerializeField] private TMP_Text _WaterLabel;
    [SerializeField] private TMP_Text _PowerLabel;

    public void UpdateFoodLabel()
    {
        _FoodLabel.text = ShipController.Instance.Resources.Food;
        _WaterLabel.text = ShipController.Instance.Resources.Water;
        _PowerLabel.text = ShipController.Instance.Resources.Power;
    }
}