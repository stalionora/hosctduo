using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

// Dependencies
// card data, CardType, IMecAction, ResourceDataRepresentation

public class ServerMock {
	//	end turn
	//	new turn
	public UnityEvent OnReversePush = new();
	public UnityEvent<CardDataImpl> OnCardPush = new();
	public UnityEvent<ResourceDataRepresentation> OnResourceTableRequest = new();
	//public Dictionary<int, CardDataImpl> DeckFirstsPlayer { get { return _deckFirstsPlayer; } }
	//public Dictionary<int, CardDataImpl> DeckSecondsPlayer { get { return _deckSecondsPlayer; } }
	public Dictionary<int, string> ResourceTable { get { return _resourceTable.ResourceTable; } }
	public int StartHandLength => _startHandLength;
	public int DeckLength => _deckLength;

	public ServerMock() {
		_cardNames = new string[] {
			"barrel",
			"worker",
			"Explosion"
		};
		_resourceTable = new (new Dictionary<int,string>() {
			{0, "card_reverse"},
			{1, _cardNames[0]},
			{2, _cardNames[1]},
			{3, _cardNames[2]}
		});
		//_cardStats = new CardDataImpl[] {
		//	//effects
		//	new CardDataImpl(_cardNames[2], new EffectCard(), 10, null, null),			// explosion effect, 1000 damage
  //          //
		//	new CardDataImpl(_cardNames[0], new StaticMobCard(), 5, null, new IMecTrigger[1]{
  //              new DeferredFigureActionTrigger(3, new IFigureMecAction[2]{new CreateFigureAction(), new SelfDestructionAction()})
  //          }),	// barrel, 5 turns to blow up, 5 damage
		//	new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null)					// worker, 1 damage
		//};
		_deckFirstsPlayer = new CardDataImpl[] {
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
            new CardDataImpl(_cardNames[1], new DirectableMobCard(), 1, null, null),					// worker, 1 damage
			new CardDataImpl(_cardNames[0], new StaticMobCard(), 5, null, new IMecTrigger[1]{
                new DeferredFigureActionTrigger(3, new IFigureMecAction[2]{new CreateFigureAction(new CardDataImpl(_cardNames[2], new EffectCard(), 10, null, null)), new SelfDestructionAction()})
            }),	// barrel, 5 turns to blow up, 5 damage
			new CardDataImpl(_cardNames[0], new StaticMobCard(), 5, null, new IMecTrigger[1]{
                new DeferredFigureActionTrigger(3, new IFigureMecAction[2]{new CreateFigureAction(new CardDataImpl(_cardNames[2], new EffectCard(), 10, null, null)), new SelfDestructionAction()})
            }),	// barrel, 5 turns to blow up, 5 damage
		};
		ConstructDecks(); 
	}

	private void ConstructDecks() {
		Debug.Log("Constructing decks");
        for (int i = 0; i < _currentDeckOneLength; ++i) {
			//_deckFirstsPlayer.Add(i, new CardDataImpl(_cardStats[i % (_cardStats.Length - 1)]));
			//_deckFirstsPlayer.Add(i, _cardStats[i]);
			_currentDeckOne[i] = i;
			//_deckSecondsPlayer.Add(i, new CardDataImpl(_cardStats[i % (_cardStats.Length - 1)]));
			//_currentDeckTwo[i] = i;
            _deckFirstsPlayer[i].FigureMechanicTriggers.ForEach(mechanic => {
                Debug.Log($"Created trigger: {mechanic.GetType().Name}, with hash {mechanic.GetHashCode()}");
            });
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

	public void PushCardData(CardDataImpl cardData) { 
 
	}
	
	//
	const int _deckLength = 10;
	const int _startHandLength = 10;
	int _buffer = 0;
	int _currentDeckOneLength = _deckLength;
	int _currentDeckTwoLength = _deckLength;
	int[] _currentDeckOne = new int[_deckLength];
	int[] _currentDeckTwo = new int[_deckLength];
	String[] _cardNames = default;
	
	//
	ResourceDataRepresentation _resourceTable;
	CardDataImpl[] _deckFirstsPlayer;
	CardDataImpl[] _deckSecondsPlayer;
	CardDataImpl[] _cardStats;
	
}

