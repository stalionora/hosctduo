using System;
using System.Collections.Generic;
using UnityEngine.Events;

// Dependencies
// card data
public class ServerMock {
	//	end turn
	//	new turn
	public UnityEvent OnReversePull = new();
	public UnityEvent<CardData> OnCardPull = new();
	public UnityEvent<ResourceDataRepresentation> OnResourceTableRequest = new();
	public Dictionary<int, CardData> DeckFirstsPlayer { get { return _deckFirstsPlayer; } }
	public Dictionary<int, CardData> DeckSecondsPlayer { get { return _deckSecondsPlayer; } }
	public Dictionary<int, string> ResourceTable { get { return _resourceTable.ResourceTable; } }

	public ServerMock() {
		_resourceTable = new(new Dictionary<int,string>() {
			{-1, "card_reverse"},
			{0, _cardNames[0]},
			{1, _cardNames[1]},
		});
		_cardNames = new string[] {
			"barrel",
			"worker",
		};
		_cardStats = new CardData[] {
			new CardData(_cardNames[0], 5),
			new CardData(_cardNames[1], 1),
		};
		//ConstructDecks(); 
	}

	public void ConstructDecks() { 
		for (int i = 0; i < _currentDeckOneLength; i++) {
			_currentDeckOne[i] = i;
			_currentDeckTwo[i] = i;
		}
	}

	public ResourceDataRepresentation OnRequestResourceTable() { 
		return _resourceTable;
	}

	public void UpdateHand() {
		//	call new turn?
		//	updating player hand
		//	updating enemy hand
		if(_currentDeckOneLength > 0) {
			_buffer = UnityEngine.Random.Range(0,_currentDeckOneLength);
			_currentDeckOne[_buffer] = _currentDeckOne[_currentDeckOneLength - 1];
			--_currentDeckOneLength;
			OnCardPull.Invoke(_deckFirstsPlayer[_buffer]);
		}
	}

	public void UpdateReversesOfSecondHand() { 
		//	call new turn?
		//	updating player hand
		//	updating enemy hand
		if(_currentDeckTwoLength > 0) {
			_currentDeckTwoLength--;
			OnReversePull.Invoke();
		}
	}

	public void PushCardData(CardData cardData) { 
 
	}
	//
	CardData[] _cardStats;
	ResourceDataRepresentation _resourceTable;

	Dictionary<int, CardData> _deckFirstsPlayer = new();
	Dictionary<int, CardData> _deckSecondsPlayer = new();
	
	String[] _cardNames = default;
	int[] _currentDeckOne = default;
	int[] _currentDeckTwo = default;
	int _buffer = 0;
	int _currentDeckOneLength = 30;
	int _currentDeckTwoLength = 30;
	readonly int _startHandLength = 8;
}

