using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

//[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObject/CardData")]
public struct CardData{
    public string Hash { get { return _hash; } }
    public int Recource { get { return _resource; } }
	public int PeopleWorth { set { _peopleWorth = value; } get { return _peopleWorth; } }

    public CardData(string hash, int peopleWorth = 0, int number = 0) {
        _hash = hash;
		_resource = number;
        _peopleWorth = peopleWorth;
	}

    public void OnCardStatsReceiving(int peopleWorth) {
        _peopleWorth = peopleWorth;
	}

	private string _hash;
    private int _resource;
	private int _peopleWorth;
}
