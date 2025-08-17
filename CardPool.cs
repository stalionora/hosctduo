using UnityEngine;

public class CardPool {
    public int Size { get {return _size; } set {_size = value; } }

    public void SetPool(int size) {
        //  card pool distributing
        _size = size;
        cards = new GameObject[size];
    }
    public GameObject[] GetPool() {
        return cards;
    } 
    
    private int _size;
    private GameObject[] cards;
}