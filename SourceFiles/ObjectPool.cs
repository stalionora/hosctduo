using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//  Game _objects managing
//  хглемхрэ сопюбкемхе пеяспянл
public class ObjectPool<T>: IEnumerable<T> {
    public int Size { get {return _size; } set {_size = value; } }

    public ObjectPool(int newSize)
    {
        SetPool(newSize * _sizeMultiplier);
    }
    ~ObjectPool() {
        for (int i = 0; i < _size; ++i)
            GameObject.Destroy(_objects[i]);
        
    }

	//  old pool should be deleted and objects copied in new one, if size is bigger than before
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
    //public GameObject[] GetPool(int amountOfElemetns) {
    //    Array.Resize(ref _objects, amountOfElemetns);
    //    return _objects;
    //}

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

    public IEnumerator GetEnumerator(){
        return new ObjectEnum<T>(_objects);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    private GameObject[] _objects;
    
    private int _size = 0;
    private int _counter = 0;
    private int _sizeMultiplier = 1;
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class ObjectEnum<T> : IEnumerator<T>{
    public GameObject[] _object;

    public ObjectEnum(GameObject[] newPool) {
        _object = newPool;
    }

    public T Current => throw new NotImplementedException();

    object IEnumerator.Current => throw new NotImplementedException();

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

}
