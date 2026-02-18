using UnityEngine;
public class ServerInteractor {
	public void Init(HandController handController) {
		Debug.Log("Server interacto init");
		_handController = handController;
		_controller.OnMultiplePush.AddListener(_handController.MultipleAdd);
		_controller.OnPushResourceTable.AddListener(GameService.GetService<CardResourceService>().SetResourceTable);
		_controller.OnPushCard.AddListener(_handController.SingleAdd);
		_controller.Init();
		_handController = handController;
		//	subcribe resource table receiver
	}

	ServerController _controller = new();
	HandController _handController;
}