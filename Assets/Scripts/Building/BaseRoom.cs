using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
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
    protected Timer _ResourceTimer;                 // Timer for generating resources

    // How long it takes for the room to produce resources
    protected float _CurrentResourceTime;       
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

}