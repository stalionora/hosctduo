using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

////////////////////////////////////////
//  Hides cards after placing on board, makes figure visible and takes data from figureDataObserver
//  Dependent from object pool, cells tracker, cellsMatrixData
//  In all prefabs object which named "Image" contains needed sprite
////////////////////////////////////////
public class FigureDistributor
{
    //  figure creation on end drag
    //  —“¿¬»“‹ ‘»√”–€ ¬ ÷≈Õ“–  À≈“ »
    public UnityEvent<GameObject> OnEndSwitching = new();
    public FigureDistributor(ObjectPool<Figure> figurePool, ObjectPool<Card> cardPool, CellsMatrixData cellsMatrixData) { 
        _figurePool = figurePool;
        _cardPool = cardPool;
        _cellsMatrixData = cellsMatrixData;
    }

    public void SwitchCardToFigure(GameObject newCurrentCard)   //  expensive parameter?
    {
        if (_figurePool == null || _cardPool == null || newCurrentCard == null) { 
            Debug.Log("Figure distributor is not initialized");
            return;
        }
        //_figurePool; 
        _figure = _figurePool.GetNextObject();

        //  circle frame
        GameObject circleFrame = new GameObject("Frame", typeof(RectTransform), typeof(Image));
        circleFrame.transform.SetParent(_figure.transform, false);
        var frameRect = circleFrame.GetComponent<RectTransform>();
        frameRect.anchorMin = Vector2.zero;
        frameRect.anchorMax = Vector2.one;
        frameRect.sizeDelta = _cellsMatrixData.CellSize * 0.6f;
        _imagesSpaceSize = frameRect.localScale.y * _percentOfImageSpaceInFrame / 100f;
        circleFrame.GetComponent<Image>().sprite = _figure.GetComponent<Image>().sprite;
        circleFrame.GetComponent<Image>().color = Color.black;

        ////  mask creation
        //GameObject maskObj = new GameObject("CircleMask", typeof(RectTransform), typeof(Image), typeof(Mask));
        //maskObj.transform.SetParent(_figure.transform, false);
        //RectTransform maskRect = maskObj.GetComponent<RectTransform>(); //  spare image in parent
        //maskRect.localScale = new Vector2(_imagesSpaceSize, _imagesSpaceSize);
        ////maskRect.sizeDelta = circleFrame.transform.localScale * 0.98f;

        //maskRect.anchorMin = Vector2.zero;
        //maskRect.anchorMax = Vector2.one;

        //// masks image
        //Image maskImage = maskObj.GetComponent<Image>();
        //maskImage.sprite = _figure.GetComponent<Image>().sprite;
        //maskImage.type = Image.Type.Simple; // ËÎË Simple, ÂÒÎË ÍÛ„Î˚È ÒÔ‡ÈÚ
        //maskImage.color = Color.white;
        //maskImage.maskable = true;

        //// figure image
        //GameObject figuresImage = new GameObject("Image", typeof(RectTransform), typeof(Image));
        //figuresImage.transform.SetParent(maskObj.transform, false);
        //RectTransform imgRect = figuresImage.GetComponent<RectTransform>();
        //imgRect.anchorMin = Vector2.zero;
        //imgRect.anchorMax = Vector2.one;

        ////  finding images
        //foreach (var spriteCard in newCurrentCard.GetComponentsInChildren<Image>())
        //    if (spriteCard.gameObject.name == "Image")
        //        foreach (var spriteFigure in _figure.GetComponentsInChildren<Image>())
        //            if (spriteFigure.gameObject.name == "Image")
        //                spriteFigure.sprite = spriteCard.sprite;

        //figuresImage.GetComponent<Image>().preserveAspect = true;   //  ?

        //_figure.transform.position = GameService.GetService<ICellsTracker>().GetCurrentCellCoordinates(); //  = position of card
        var cellsTracker = GameService.GetService<ICellsTracker>();
        cellsTracker.CalcuateCurrentCell(newCurrentCard.transform.position);
        _figure.transform.position = cellsTracker.GetCurrentCellCoordinates(); //  = position of card

        _cardPool.ReturnObject(newCurrentCard);
        _figure.SetActive(true);
        OnEndSwitching.Invoke(_figure);
    }



    ObjectPool<Figure> _figurePool;
    ObjectPool<Card> _cardPool;
    GameObject _figure;
    CellsMatrixData _cellsMatrixData;

    float _imagesSpaceSize = 0f;   
    const float _percentOfImageSpaceInFrame = 90f;
}