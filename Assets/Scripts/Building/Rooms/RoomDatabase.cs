using UnityEngine;
using System.Collections.Generic;


public class RoomDatabase : MonoBehaviour 
{
    public static RoomDatabase Instance;
    [SerializeField] private List<RoomData> _Rooms = new List<RoomData>();

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
    }

    public RoomData GetRoom(string roomName)
    {
        foreach(var room in _Rooms)
        {
            if(room.RoomName == roomName)
                return room;
        }

        return null;
    }
}