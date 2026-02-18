using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class HandController: MonoBehaviour{
	[SerializeField]
	RectTransform PlayersHandRect;
	[SerializeField]
	GameObject CardPrefab;
	public UnityEvent<GameObject> CardReadyToPlant = new();

	//	pools should be created
	public void Init(IFabric cardFabric, HandScale handScale) {
		Debug.Log("Hand controller init");
		_hand = GetComponent<Hand>();
		_hand.Init(handScale.StartingHandSize);  //	hand model init with 0 cards in it
		_cardFabric = cardFabric;
		_dealer = GetComponent<HandDistributor>();
		_dealer.Initialize();    //  inherited from image in nested canvas
		//_hand.OnUpdatingHand.AddListener(_dealer.DistributeCards);   //  subscribe to hand update event for visual distribution
	}

	public void MultipleAdd(params CardData[] cardsParams) {
		Debug.Log($"Adding {cardsParams.Length} cards in the hand");

		for(int i = 0; i < cardsParams.Length; ++i) {
			//if(cardsParams[i].GameObject() != null)
				SetStatsToNextCardFromPool(cardsParams[i]);
		}
		_dealer.DistributeCards(_hand.CardsList);   //  visual distribution
	}

	public void SingleAdd(CardData cardData) {
		SetStatsToNextCardFromPool(cardData);
		_dealer.DistributeCards(_hand.Cards);   //  visual distribution
	}

	//	receices stats from server interactor and creades card with them
	private void SetStatsToNextCardFromPool(CardData stats) {
		Debug.Log("Setting card stat");
		var tempObj = _cardFabric.Create(new Vector3());
		tempObj.AddComponent<Card>().SetStats(stats);
		_hand.AddCard(tempObj); //	adding in model for further use
								//CardReadyToPlant.Invoke(tempObj);	//	related actions after stats are set 
	}


	private Hand _hand;
	private IFabric _cardFabric;
	private HandDistributor _dealer;
	private CardFuncController _cardsStatesController;
}