using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
//  Game _objects managing
//  хглемхрэ сопюбкемхе пеяспянл
public class ObjectPool<T> {
    public int Size { get {return _size; } set {_size = value; } }

    public ObjectPool(int newSize)
    {
        SetPool(newSize);
    }
    ~ObjectPool() {
        for (int i = 0; i < _size; ++i)
            GameObject.Destroy(_objects[i]);
        
    }

    public void SetPool(int size){
            Debug.Log($"setting pool[{size}]");
        //  card pool distributing
        if (size > _size){
            _objects = new GameObject[size];
            _size = size;
        }
    }
    //  shit
    public GameObject[] GetPool() {
        return _objects;
    }
    public GameObject[] GetPool(int amountOfElemetns) {
        Array.Resize(ref _objects, amountOfElemetns);
        return _objects;
    }

    public GameObject GetNextObject() {
        try {
            if (_objects.Length > _counter)
                return _objects[_counter++];
            else
                throw new IndexOutOfRangeException("overflowing of pool");
        }
        catch (IndexOutOfRangeException){
            return null; 
        }
    }
    
    public void ReturnObject(GameObject newObject){
        if (newObject.GetComponent<T>() != null){
            _objects.Append(newObject);
            newObject.SetActive(false);
        }
        else
            throw new Exception("wrong type of returning object in pool");
    }

    private GameObject[] _objects;
    
    private int _size = 0;
    private int _counter = 0;
}