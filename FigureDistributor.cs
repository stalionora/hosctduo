using UnityEngine;

////////////////////////////////////////
//  Dependent from object pool
////////////////////////////////////////
public class FigureDistributor
{
    //  figure creation on end drag

    public FigureDistributor(ObjectPool<Figure> figurePool, ObjectPool<Card> cardPool) { 
        _figurePool = figurePool;
        _cardPool = cardPool;
    }

    public void SwitchCardToFigure(GameObject newCurrentCard)
    {
        if (_figurePool == null || _cardPool == null || newCurrentCard == null) { 
            Debug.Log("Figure distributor is not initialized");
            return;
        }
        _currentCard = newCurrentCard;
        //_figurePool; 
        _cardPool.ReturnObject(_currentCard);
        figure = _figurePool.GetNextObject();
        figure.transform.position = _currentCard.transform.position; //  = position of card
        figure.SetActive(true);
    }

    GameObject _currentCard;
    ObjectPool<Figure> _figurePool;
    ObjectPool<Card> _cardPool;
    GameObject figure;
}