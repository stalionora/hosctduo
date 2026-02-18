using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
//using UnityEngine.Events;

//  model which only contains cards from pool

public class Hand : MonoBehaviour{
	//public UnityEvent<GameObject[]> OnUpdatingHand = new UnityEvent<GameObject[]>();
	public GameObject[] Cards { get { return _cards; } set { _cards = value; } }
	public GameObject[] CardsList { get { return _cardList.ToArray(); } }
    public void Init(int startingHandSize) {
		_cards = new GameObject[startingHandSize];
		_emptySlotNum = 0;
	}

    //public void AddCard(params GameObject[] cardsParam) {
		//if(_emptySlotNum + cardsParam.Length >= _cards.Length) {
		//	var tmp = new GameObject[_cards.Length + cardsParam.Length];
		//	for(int i = 0; i < _cards.Length; i++) {
		//		tmp[i] = _cards[i];
		//	}
		//	for(int i = 0; i < cardsParam.Length; ++i) {
		//		tmp[i + _cards.Length] = cardsParam[i];
		//	}
		//	_cards = tmp;
		//	//	delete Array
		//}
		//else {
		//	for(int i = 0; i < cardsParam.Length; ++i)
		//		_cards[_emptySlotNum++] = cardsParam[i];
		//}
		////OnUpdatingHand.Invoke(_cards);
	//}
    public void AddCard(params GameObject[] cardsParam) {
		if(cardsParam.Length >= 1)
		foreach(var card in cardsParam) {
			_cardList.Add(card);
		}
		//OnUpdatingHand.Invoke(_cardsList);
	}


	public void RemoveCard(GameObject cardsParam) {
		var tmp = new GameObject[_cards.Length - 1];
		for(int i = 0; i < _cards.Length - 1; i++) {
			tmp[i] = _cards[i];
		}
		_cards = tmp;
	}

	private List<GameObject> _cardList = new();
	private GameObject[] _cards = new GameObject[0]; //  pool's resources + current amount of cards
	int _emptySlotNum = 0;

}
