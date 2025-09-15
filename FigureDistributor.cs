using Representation;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
////////////////////////////////////////
//  Hides cards after placing on board, makes figure visible and takes data from figureDataObserver
//  Dependent from object pool, cells tracker, cellsMatrixData
//  In all prefabs object which named "Image" contains needed sprite
////////////////////////////////////////
public class FigureDistributor
{
    //  figure creation on end drag
    //  СТАВИТЬ ФИГУРЫ В ЦЕНТР КЛЕТКИ
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
        figure = _figurePool.GetNextObject();

        //  circle frame
        GameObject circleFrame = new GameObject("Frame", typeof(RectTransform), typeof(Image));
        circleFrame.transform.SetParent(figure.transform, false);
        var frameRect = circleFrame.GetComponent<RectTransform>();
        frameRect.anchorMin = Vector2.zero;
        frameRect.anchorMax = Vector2.one;
        frameRect.sizeDelta = _cellsMatrixData.CellSize * 0.57f;
        circleFrame.GetComponent<Image>().sprite = figure.GetComponent<Image>().sprite;
        circleFrame.GetComponent<Image>().color = Color.black;
        // --- Создаём объект для маски ---
        GameObject maskObj = new GameObject("CircleMask", typeof(RectTransform), typeof(Image), typeof(Mask));
        maskObj.transform.SetParent(figure.transform, false);
        RectTransform maskRect = maskObj.GetComponent<RectTransform>(); //  spare image in parent
        maskRect.sizeDelta = circleFrame.transform.localScale * 0.9f;
        
        maskRect.anchorMin = Vector2.zero;
        maskRect.anchorMax = Vector2.one;

        // --- Настраиваем Image маски ---
        Image maskImage = maskObj.GetComponent<Image>();
        maskImage.sprite = figure.GetComponent<Image>().sprite;
        maskImage.type = Image.Type.Simple; // или Simple, если круглый спрайт
        maskImage.color = Color.white;
        maskImage.maskable = true;

        // --- Создаём Image с фигурой внутри маски ---
        GameObject figuresImage = new GameObject("Image", typeof(RectTransform), typeof(Image));
        figuresImage.transform.SetParent(maskObj.transform, false);
        RectTransform imgRect = figuresImage.GetComponent<RectTransform>();
        imgRect.anchorMin = Vector2.zero;
        imgRect.anchorMax = Vector2.one;

        //  finding images
        foreach (var spriteCard in newCurrentCard.GetComponentsInChildren<Image>())
            if (spriteCard.gameObject.name == "Image")
                foreach (var spriteFigure in figure.GetComponentsInChildren<Image>())
                    if (spriteFigure.gameObject.name == "Image")
                        spriteFigure.sprite = spriteCard.sprite;

        figuresImage.GetComponent<Image>().preserveAspect = true;   //  ?
        
        figure.transform.position = GameService.GetService<ICellsTracker>().GetCurrentCellCoordinates(); //  = position of card
        _cardPool.ReturnObject(newCurrentCard);
        figure.SetActive(true);
    }

    ObjectPool<Figure> _figurePool;
    ObjectPool<Card> _cardPool;
    GameObject figure;
    CellsMatrixData _cellsMatrixData;
}