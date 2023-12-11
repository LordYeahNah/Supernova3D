using System;
using System.Collections.Generic;
using UnityEngine;

public class ColonyResources
{
    // === Current Stats === //
    private float _CurrentFood;
    private float _CurrentPower;
    private float _CurrentWater;

    public float CurrentFood => _CurrentFood;
    public float CurrentPower => _CurrentPower;
    public float CurrentWater => _CurrentWater;
    
    
    // === Max Stats === //
    private float _MaxFood;
    private float _MaxWater;
    private float _MaxPower;
    

    public float MaxFood => _MaxFood;
    public float MaxWater => _MaxWater;
    public float MaxPower => _MaxPower;
    
    // === Min Stats === //
    private float _MinFood;
    private float _MinWater;
    private float _MinPower;

    public float MinFood => _MinFood;
    public float MinWater => _MinWater;
    public float MinPower => _MinPower;
    
    
    // === Strings === //
    public string Water => $"{Mathf.FloorToInt(_CurrentWater)}/{Mathf.FloorToInt(_MaxWater)}";
    public string Power => $"{Mathf.FloorToInt(_CurrentPower)}/{Mathf.FloorToInt(_MaxPower)}";
    public string Food => $"{Mathf.FloorToInt(_CurrentFood)}/{Mathf.FloorToInt(_MaxFood)}";
    
    // === Modifiers === //
    public float _PowerModifier = 1.0f;
    
    // === Timers === //
    private Timer _UsageTimer;

    public ColonyResources()
    {
        _UsageTimer = new Timer(1.0f, HandleUsage, true, false);
    }

    public ColonyResources(float currentFood, float currentWater, float currentPower, List<BaseColonist> colonistList)
    {
        _CurrentFood = currentFood;
        _CurrentWater = currentWater;
        _CurrentPower = currentPower;

        CalculateFoodAndWater(colonistList);
        _UsageTimer = new Timer(1.0f, HandleUsage, true, false);
    }

    public void CalculateFoodAndWater(List<BaseColonist> colonistList)
    {
        _MinFood = 0f;
        _MinWater = 0f;
        foreach (var colonist in colonistList)
        {
            _MinWater += colonist.WaterUsagePerSecond;
            _MinFood += colonist.FoodUsagePerSecond;
        }
    }

    public void CalculatePower(List<BaseRoom> roomList)
    {
        _MinPower = 0f;
        foreach (var room in roomList)
        {
            _MinPower += room.PowerPerSecond();
        }
    }

    public void OnUpdate(float dt)
    {
        if(_UsageTimer != null)
            _UsageTimer.OnUpdate();
    }

    private void HandleUsage()
    {
        _CurrentFood -= _MinFood;
        _CurrentWater -= _MinWater;
        _CurrentPower -= _MinPower;
    }

    public void AddPower(int value)
    {
        _CurrentPower += value;
        if (_CurrentPower > _MaxPower)
            _CurrentPower = _MaxPower;
    }
}