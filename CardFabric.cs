using UnityEngine;

public class CardFabric: IFabric {

	public CardFabric() { }
	public CardFabric(GameObject prefab, Transform canvas) {
		_cardPrefab = prefab;
		_handCanvas = canvas;
	}

	public GameObject Create(Vector3 pointToPlace){
		GameObject newCard;
		if (_cardPrefab != null){
			newCard = GameObject.Instantiate(_cardPrefab);
		}
		else{
			newCard = new GameObject();
			newCard.AddComponent<SpriteRenderer>();
		}
		newCard.AddComponent<CardDragHandler>();
		newCard.transform.SetParent(_handCanvas);
		return newCard;
	}

	private GameObject _cardPrefab;
	private Transform _handCanvas;
}