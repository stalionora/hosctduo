using UnityEngine;
public class HandController{
	public HandController(Hand hand, ObjectPool<Card> pool, CardFabric cardFabric, HandDistributor dealer, CardFuncController fsm) {
		_hand = hand;
		_cardPool = pool;
		_cardFabric = cardFabric;
		_cardsStatesController = fsm;

	}

	public void Add(params CardData[] cards) {
		_cardsStatesController.SetSwitchingRuleOfExecutionForCurrentCard(_cardFabric.Create(new Vector3()).GetComponent<CardDragHandler>());

	}

	public void RemoveCard() { 
	}

	public void SetStatsToNextCardFromPool(CardData stats) {
		_cardPool.GetNextObject().GetComponent<Card>().SetStats(stats);
	}

	public void UpdateHand() { 
		
	}

	private Hand _hand;
	private ObjectPool<Card> _cardPool;
	private CardFabric _cardFabric;
	private HandDistributor _dealer;
	private CardFuncController _cardsStatesController;
}