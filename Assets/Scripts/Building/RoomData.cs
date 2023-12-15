using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public string RoomName;                             // Name of the room
    public EResourceType ResourceType;                          // Type of resource that is produced
    public float _PowerUsagePerSecond;                      // How much power this room uses per second
    public float BaseTimePerRoomSize;                           // How long this room takes to generate resources

    [Tooltip("How many resources are generated when ready for collection")]
    public int ResourcesGenerated;                              // How many resources are generated when ready for collection
    public int StoragePerCell;

    [Header("Room Objects")]
    public GameObject Wall;
    public GameObject Floor;
    public GameObject Corner;
    public GameObject Door;

    [Header("Room Sizing")]
    public int MinCellsX;
    public int MinCellsY;
}