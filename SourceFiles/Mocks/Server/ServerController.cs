using UnityEngine.Events;
using System.Collections.Generic;
//Dependencies
//server mock, data representation

public class ServerController {
	public UnityEvent OnPushReverse = new();
	public UnityEvent<CardData> OnPushCard = new();
	public UnityEvent<ResourceDataRepresentation> OnPushResourceTable = new();
	public UnityEvent<CardData[]> OnMultiplePush = new();

	public void Init() { 
		_resourcesTable = new ResourceDataRepresentation(_mock.ResourceTable);
		_mock.OnCardPush.AddListener(PrepareMultipleDatasToPush);
		_mock.OnStart();
		OnMultiplePush.Invoke(_cardDatas.ToArray());
		_mock.OnReversePush.AddListener(OnReversPush);    //card distributor
		OnPushResourceTable.Invoke(_resourcesTable);
		GameService.GetService<TimerMock>().OnTurnEnd.AddListener(OnEndTurn);
		_mock.OnCardPush.RemoveListener(PrepareMultipleDatasToPush);
		_mock.OnCardPush.AddListener(OnPush);   //cardDistributor, Hand
		//_cardDatas.Clear();
	}

	private void OnEndTurn() {
		_mock.UpdateHand();
		//_mock.UpdateReversesOfSecondHand();
	}

	private void OnPush(CardData data) {
		OnPushCard.Invoke(data);
	}
	
	private void OnReversPush() {
		OnPushReverse.Invoke();
	}
	
	private void PrepareMultipleDatasToPush(CardData data) {
		_cardDatas.Add(data);
	}

	private ServerMock _mock = new();
	private ResourceDataRepresentation _resourcesTable;
	private List<CardData> _cardDatas = new List<CardData>();
}