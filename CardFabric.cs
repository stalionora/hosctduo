using NUnit.Framework;
using UnityEngine;

//////////////////////////
// Dependent from DragHandler
//////////////////////////
public class CardFabric: IFabric {
	public CardFabric(GameObject prefab, RectTransform canvas = null) {
		Assert.IsNotNull(prefab);
		_cardPrefab = prefab;
		_handCanvas = canvas;
	}

	public GameObject Create(Vector3 pointToPlace){
		GameObject newCard;
		if (_cardPrefab != null){
			if (_handCanvas != null){ 
				newCard = GameObject.Instantiate(_cardPrefab, _handCanvas);
				//newCard.transform.SetParent(_handCanvas, false);
			}
			else 
				newCard = GameObject.Instantiate(_cardPrefab);
			newCard.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
            //newCard.GetComponentInChildren<UnityEngine.UI.Image>().raycastTarget = false;
        }
		else{
            Debug.LogError("CardFabric: _cardPrefab is null");
            newCard = new GameObject();
			newCard.AddComponent<SpriteRenderer>();
		}
		CardDragHandler dragHandler = newCard.GetComponent<CardDragHandler>();
		if (dragHandler == null)
		{
			dragHandler = newCard.AddComponent<CardDragHandler>();
			dragHandler = new CardDragHandler();
		}
		//var movementHandler = newCard.AddComponent<CardMovementService>();
		newCard.SetActive(false);
        return newCard;
	}

    private GameObject _cardPrefab;
	private RectTransform _handCanvas;
}