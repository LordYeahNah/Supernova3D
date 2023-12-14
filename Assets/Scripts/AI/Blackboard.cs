using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Blackboard
{

    private List<BlackboardProperty> _Properties = new List<BlackboardProperty>();

    public void SetValue<T>(string key, T value)
    {
        foreach(var prop in _Properties)
        {
            if(prop.Key == key)
            {
                if(prop is BlackboardValue<T> propValue)
                {
                    propValue.Value = value;
                    return;
                }
            }
        }

        _Properties.Add(new BlackboardValue<T>(key, value));
    }

    public T GetValue<T>(string key)
    {
        foreach(var prop in _Properties)
        {
            if(prop.Key == key)
            {
                if(prop is BlackboardValue<T> propValue)
                    return propValue.Value;
            }
        }

        return default;
    }

    private class BlackboardProperty
    {
        public string Key;

        public BlackboardProperty(string key)
        {
            Key = key;
        }
    }

    private class BlackboardValue<T> : BlackboardProperty
    {
        public T Value;

        public BlackboardValue(string key, T value) : base(key)
        {
            Value = value;
        }
    }
}