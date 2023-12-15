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

    private ShipController _Owner;                          // Reference to the ship that owns this object

    public ColonyResources(ShipController ship)
    {
        _UsageTimer = new Timer(1.0f, HandleUsage, true, false);
        _Owner = ship;
        _Owner._OnResourcesUpdate?.Invoke();
    }

    public ColonyResources(ShipController ship, float currentFood, float currentWater, float currentPower, List<BaseColonist> colonistList)
    {
        _CurrentFood = currentFood;
        _CurrentWater = currentWater;
        _CurrentPower = currentPower;

        CalculateFoodAndWater(colonistList);
        _UsageTimer = new Timer(1.0f, HandleUsage, true, true);

        _Owner = ship;
        _Owner._OnResourcesUpdate?.Invoke();
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

        if(_Owner != null)
            _Owner._OnResourcesUpdate?.Invoke();
    }

    public void CalculateMaxFoodAndWater(List<BaseRoom> rooms)
    {
        _MaxWater = 0f;
        _MaxFood = 0f;
        foreach(var room in rooms)
        {
            if(room.Data.ResourceType == EResourceType.FOOD)
            {
                _MaxFood += room.StorageAmount;
            } else if(room.Data.ResourceType == EResourceType.WATER)
            {
                _MaxWater += room.StorageAmount;
            }
        }
    }

    public void CalculatePower(List<BaseRoom> roomList)
    {
        _MinPower = 0f;
        _MaxPower = 0f;
        foreach (var room in roomList)
        {
            _MinPower += room.PowerPerSecond();
            if(room.Data.ResourceType == EResourceType.POWER)
                _MaxPower += room.StorageAmount;
        }

        if(_Owner != null)
            _Owner._OnResourcesUpdate?.Invoke();
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

        if(_CurrentFood < _MinFood)
        {
            // TODO: Start reducing happiness of colonist
            if(_CurrentFood <= 0)
            {
                // TODO: Start reducing Health
                _CurrentFood = 0f;
            }
        }
            

        if(_CurrentWater < _MinWater)
        {
            // TODO: Start reducing happiness of colonist
            if(_CurrentWater <= 0)
            {
                // TODO: Start reducing Health
                _CurrentWater = 0f;
            }
        }

        if(_CurrentPower < _MinPower)
        {
            // TODO: Power down rooms furthest away
            if(_CurrentPower < 0)
            {
                // TODO: Start Reducing Health
                _CurrentPower = 0f;
            }
        }

        if(_Owner != null)
            _Owner._OnResourcesUpdate?.Invoke();
    }

    public void AddPower(int value)
    {
        _CurrentPower += value;
        if (_CurrentPower > _MaxPower)
            _CurrentPower = _MaxPower;

        if(_Owner != null)
        {
            _Owner._OnResourcesUpdate?.Invoke();
        }
        
    }

    public void AddWater(int value)
    {
        _CurrentWater += value;
        if(_CurrentWater > _MaxWater)
            _CurrentWater = _MaxWater;

        if(_Owner != null)
            _Owner._OnResourcesUpdate?.Invoke();
    }

    public void AddFood(int value)
    {
        _CurrentFood += value;
        if(_CurrentFood > _MaxFood)
            _CurrentFood = _MaxFood;

        if(_Owner != null)
            _Owner._OnResourcesUpdate?.Invoke();
    }
}