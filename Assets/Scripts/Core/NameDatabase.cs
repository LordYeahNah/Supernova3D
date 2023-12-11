using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameDatabase : MonoBehaviour
{
    public static NameDatabase Instance;

    [SerializeField] private List<string> _MaleNames = new List<string>();
    [SerializeField] private List<string> _FemaleNames = new List<string>();
    [SerializeField] private List<string> _LastNames = new List<string>();

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

    public string GetMaleName() => _MaleNames[Random.Range(0, _MaleNames.Count)];
    public string GetFemaleName() => _FemaleNames[Random.Range(0, _FemaleNames.Count)];
    public string GetLastName() => _LastNames[Random.Range(0, _LastNames.Count)];
}
