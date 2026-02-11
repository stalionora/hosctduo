using UnityEngine;
public class ServerInteractor {
	public void Init(HandController handController) {
		_controller.OnStart();
		_handController = handController;
		
		_controller.OnPushCard.AddListener(_handController.SetStatsToNextCardFromPool);
	
	}

	ServerController _controller = new();
	HandController _handController;
}