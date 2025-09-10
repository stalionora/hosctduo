using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> {
    public int Size { get {return _size; } set {_size = value; } }

    public ObjectPool(int newSize)
    {
        SetPool(newSize);
    }
    ~ObjectPool() {
        for (int i = 0; i < _size; ++i)
            GameObject.Destroy(objects[i]);
        
    }

    public void SetPool(int size){
            Debug.Log($"setting pool[{size}]");
        //  card pool distributing
        if (size > _size){
            objects = new GameObject[size];
            _size = size;
        }
    }
    //  shit
    public GameObject[] GetPool() {
        return objects;
    }

    public GameObject GetNextObject() {
        try {
            if (objects.Length > counter)
                return objects[counter++];
            else
                throw new IndexOutOfRangeException("overflowing of pool");
        }
        catch (IndexOutOfRangeException){
            return null; 
        }
    }
    
    public void ReturnObject(GameObject newObject){
        if (newObject.GetComponent<T>() != null){
            objects.Append(newObject);
            newObject.SetActive(false);
        }
        else
            throw new Exception("wrong type of returning object in pool");
    }

    private GameObject[] objects;
    
    private int _size = 0;
    private int counter = 0;
}