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
				newCard.transform.SetParent(_handCanvas, false);
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
		//CardDragHandler dragHandler = newCard.GetComponent<CardDragHandler>();
		//if (dragHandler == null){
		//	dragHandler = newCard.AddComponent<CardDragHandler>();
		//}
		
		newCard.SetActive(false);
        return newCard;
	}

    private GameObject _cardPrefab;
	private RectTransform _handCanvas;
}

class FigureFabric : IFabric
{
    public FigureFabric(GameObject prefab, RectTransform canvas = null, Figure figureType = null)
    {
        Assert.IsNotNull(prefab);
        _prefab = prefab;
        _fieldCanvas = canvas;
    }
    public GameObject Create(Vector3 coordinates)
    {
        GameObject newFigure;
        //
        if (_prefab == null)
            Debug.Log("null in fabric");
        //
        if (_fieldCanvas != null)
            newFigure = GameObject.Instantiate(_prefab, _fieldCanvas);
        else
            newFigure = GameObject.Instantiate(_prefab);
        GameService.GetService<CardResourceService>().LoadCardsResource();
        ///////////////////////////////////////////////////////////////////////////////
        // -> -> -> _prefab.transform.localScale = scale of cells <- <- <-
        ///////////////////////////////////////////////////////////////////////////////

        //GameObject uiChild = new GameObject("UI_Image", typeof(RectTransform), typeof(CanvasRenderer), typeof(UnityEngine.UI.Image));

        //uiChild.transform.SetParent(newFigure.transform, false);

        //// Настраиваем RectTransform
        //RectTransform rt = uiChild.GetComponent<RectTransform>();
        //rt.anchorMin = Vector2.zero;
        //rt.anchorMax = Vector2.one;
        //rt.offsetMin = Vector2.zero;
        //rt.offsetMax = Vector2.zero;

        //// Настраиваем Image
        //UnityEngine.UI.Image img = uiChild.GetComponent<UnityEngine.UI.Image>();
        //img.raycastTarget = true;
        //img.color = Color.black;
        newFigure.SetActive(false);
        return newFigure;
    }
    private RectTransform _fieldCanvas;
    private GameObject _prefab;
    private Figure _figureType;
}