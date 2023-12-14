using UnityEngine;
using System.Collections.Generic;

public enum EAddColonistFlag
{
    SUCCESS = 0,
    FULL = 1,
    ERROR = 2
}

public class ColonyController : MonoBehaviour
{

    public static ColonyController Instance;

    public bool IsInBuildMode = false;
    private List<BaseColonist> _Colonist = new List<BaseColonist>();                    // List of all colonist in the colony
    
    public ColonyResources Resources;                  // TODO: Rename as public variable

    private List<BaseRoom> _Rooms = new List<BaseRoom>();

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

        Resources = new ColonyResources(10, 10, 10, _Colonist);
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
            Resources.CalculateFoodAndWater(_Colonist);                    // Recalculate colonist
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
        Resources.CalculatePower(_Rooms);
    } 
    public void RemoveRoom(BaseRoom room) => _Rooms.Remove(room);
}