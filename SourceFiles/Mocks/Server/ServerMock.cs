using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

// Dependencies
// card data
public class ServerMock {
	//	end turn
	//	new turn
	public UnityEvent OnReversePush = new();
	public UnityEvent<CardData> OnCardPush = new();
	public UnityEvent<ResourceDataRepresentation> OnResourceTableRequest = new();
	public Dictionary<int, CardData> DeckFirstsPlayer { get { return _deckFirstsPlayer; } }
	public Dictionary<int, CardData> DeckSecondsPlayer { get { return _deckSecondsPlayer; } }
	public Dictionary<int, string> ResourceTable { get { return _resourceTable.ResourceTable; } }

	public ServerMock() {
		_cardNames = new string[] {
			"barrel",
			"worker",
		};
		_resourceTable = new (new Dictionary<int,string>() {
			{-1, "card_reverse"},
			{0, _cardNames[0]},
			{1, _cardNames[1]},
		});
		_cardStats = new CardData[] {
			new CardData(_cardNames[0], 5),
			new CardData(_cardNames[1], 1),
		};
		ConstructDecks(); 
	}

	private void ConstructDecks() {
		Debug.Log("Constructing decks");
		for(int i = 0; i < _currentDeckOneLength; ++i) {
			_deckFirstsPlayer.Add(i, _cardStats[i % _cardStats.Length]);
			_deckSecondsPlayer.Add(i, _cardStats[i % _cardStats.Length]);
			_currentDeckOne[i] = i;
			_currentDeckTwo[i] = i;
		}
	}

	public ResourceDataRepresentation OnRequestResourceTable() { 
		return _resourceTable;
	}


	public void UpdateHand(int AmountOfCardsToMake = 0) {
		Debug.Log("Updating hand");
		//	call new turn?
		//	updating player hand
		//	updating enemy hand
		if(_currentDeckOneLength > 0) {
			_buffer = UnityEngine.Random.Range(0,_currentDeckOneLength);
			_currentDeckOne[_buffer] = _currentDeckOne[_currentDeckOneLength - 1];
			--_currentDeckOneLength;
			OnCardPush.Invoke(_deckFirstsPlayer[_buffer]);
		}
	}

	public void OnStart() {
		Debug.Log("starting server");
		for(int i = 0; i < _startHandLength; ++i)
			UpdateHand();
	}
	//public void OnStartGame() {
	//	_buffer = UnityEngine.Random.Range(0,_currentDeckOneLength);
	//	_currentDeckOne[_buffer] = _currentDeckOne[_currentDeckOneLength - 1];
	//	--_currentDeckOneLength;
	//}

	public void UpdateReversesOfSecondHand() { 
		Debug.Log("Updating enemys hand");
		//	call new turn?
		//	updating player hand
		//	updating enemy hand
		if(_currentDeckTwoLength > 0) {
			_currentDeckTwoLength--;
			OnReversePush.Invoke();
		}
	}

	public void PushCardData(CardData cardData) { 
 
	}
	
	//
	const int _deckLength = 30;
	const int _startHandLength = 3;
	String[] _cardNames = default;
	int _buffer = 0;
	int _currentDeckOneLength = _deckLength;
	int _currentDeckTwoLength = _deckLength;
	int[] _currentDeckOne = new int[_deckLength];
	int[] _currentDeckTwo = new int[_deckLength];
	
	//
	CardData[] _cardStats;
	ResourceDataRepresentation _resourceTable;

	Dictionary<int, CardData> _deckFirstsPlayer = new();
	Dictionary<int, CardData> _deckSecondsPlayer = new();
	
}

