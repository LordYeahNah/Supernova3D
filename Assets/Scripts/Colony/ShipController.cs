using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public enum EAddColonistFlag
{
    SUCCESS = 0,
    FULL = 1,
    ERROR = 2
}

public class ShipController : MonoBehaviour
{

    public static ShipController Instance;

    public bool IsInBuildMode = false;
    private List<BaseColonist> _Colonist = new List<BaseColonist>();                    // List of all colonist in the colony
    
    public ColonyResources Resources;                  // TODO: Rename as public variable

    private List<BaseRoom> _Rooms = new List<BaseRoom>();
    public UnityEvent _OnResourcesUpdate;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else 
        {
            Destroy(this.gameObject);
        }

        Resources = new ColonyResources(this, 10, 10, 10, _Colonist);
        Resources.CalculateFoodAndWater(_Colonist);
    }

    private void Update()
    {
        if(Resources != null)
            Resources.OnUpdate(Time.deltaTime);

        // Update each room
        foreach (var room in _Rooms)
        {
            if(room != null)
                room.OnUpdate();
        }

        // Update all the settlers
        foreach(var colonist in _Colonist)
        {
            if(colonist != null)
                colonist.OnUpdate();
        }
    }
    

    /// <summary>
    /// Adds a new colonist to the colony
    /// </summary>
    /// <param name="colonist">Colonist to add</param>
    public EAddColonistFlag AddColonist(BaseColonist colonist)
    {
        // TODO: Validate there is space available to add colonist
        if(colonist != null && !_Colonist.Contains(colonist))
        {
            _Colonist.Add(colonist);
            if(Resources != null)
            {
                Resources.CalculateFoodAndWater(_Colonist);                    // Recalculate colonist
            }
            return EAddColonistFlag.SUCCESS;
        }

        return EAddColonistFlag.ERROR;
    }

    /// <summary>
    /// Removes a colonist from the colony
    /// </summary>
    /// <param name="colonist">Colonist to remove</param>
    public void RemoveColonist(BaseColonist colonist)
    {
        if(colonist != null && _Colonist.Contains(colonist))
            _Colonist.Remove(colonist);
    }

    public void AddRoom(BaseRoom room)
    {
        _Rooms.Add(room);
        if(Resources != null)
        {
            Resources.CalculatePower(_Rooms);
            Resources.CalculateMaxFoodAndWater(_Rooms);
            _OnResourcesUpdate?.Invoke();
        }
        
    } 
    public void RemoveRoom(BaseRoom room) => _Rooms.Remove(room);
}