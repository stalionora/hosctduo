using Unity.VisualScripting;
using UnityEngine.Events;
//Dependencies
//server mock, data representation

public class ServerController {
	public UnityEvent OnPushReverse = new();
	public UnityEvent<CardData> OnPushCard = new();
	public UnityEvent<ResourceDataRepresentation> OnPushResourceTable = new();

	public void Init() { 
		_mock = new ServerMock();
		_data = new ResourceDataRepresentation(_mock.ResourceTable);
		GameService.GetService<TimerMock>().OnTurnEnd.AddListener(OnEndTurn);
		_mock.OnCardPull.AddListener(OnCardPull);	//cardDistributor, Hand
		_mock.OnReversePull.AddListener(OnReversedPull);	//card distributor
	}

	public ResourceDataRepresentation OnStart() { 
		return _data;
	}

	public void OnEndTurn() {
		_mock.UpdateHand();
		_mock.UpdateReversesOfSecondHand();
	}
	
	public void OnCardPull(CardData data) {
		OnPushCard.Invoke(data);
	}
	
	public void OnReversedPull() {
		OnPushReverse.Invoke();
	}

	private ServerMock _mock;
	private ResourceDataRepresentation _data;
}