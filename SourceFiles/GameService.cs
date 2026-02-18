using System.Collections.Generic;
using System;
using UnityEngine;
public class GameService

{
    static public void Clear()
    {
        _services.Clear();
    }
    static public void Initialize() 
    {
        _services = new Dictionary<string, object>();
    }
    static public T GetService<T> () 
    {
        if (_services.ContainsKey(typeof(T).Name)){
            Debug.Log("Game service " + typeof(T).Name + " was received");
            return (T)(_services[typeof(T).Name]);
        }
        else
            throw new Exception("Ivnalid service request");
    }
    static public T Register<T>(T newObject) 
    {
        _services.Add(typeof(T).Name, newObject);
        Debug.Log("New service " + typeof(T).Name + " was registered");
        return (T)_services[typeof(T).Name];
    }

    // fields
    static private Dictionary<string, object> _services;

}