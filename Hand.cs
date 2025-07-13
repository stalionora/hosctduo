using UnityEngine;
//////////////////////////
// Dependent from GameService, each service
//////////////////////////
public class Hand : MonoBehaviour {
    //  interface
    [SerializeField]
    private CardSize CardSize;
    [SerializeField]
    private int HandSize;
    [SerializeField]
    private float OffsetBetweenCards;
    [SerializeField]
    private float RightLeftShift;
    
    public IFabric CardFabric { get {return _cardFabric; } set { _cardFabric = value; } }
    

    public void Initialize()
    {
        Debug.Log("Hand initialization");
        if (CardSize == null || _cardFabric == null)
            Debug.Log("SerializeFields was not assigned");
        _handCorners = new Vector3[4];
        for (int i = 0; i < 4; ++i)
            _handCorners[i] = new Vector3();    //  нужно, а то будут nulls
        GetComponentInParent<RectTransform>().GetWorldCorners(_handCorners);
        //  determination of the bounds
        //  -s- ... -c- o -c- o -c- ... -s-     -->     ... -c- o -c- ...
        _overflownHandSize = (int)( (_handCorners[3].x - _handCorners[0].x - RightLeftShift * 2) / (CardSize.CardScale.x / 2) );
        _fullHandSize = (int)((_handCorners[3].x - _handCorners[0].x - RightLeftShift * 2) / (CardSize.CardScale.x + OffsetBetweenCards));  //  меньше на один оффсет чем должно быть 
        if (CardSize.CardScale.x / 2 * HandSize + RightLeftShift * 2 > _handCorners[3].x - _handCorners[0].x)    //  прервать выполнение, если карт больше чем длина пространства деленная на ширину одной карты
            Debug.Log("Больше карт чем можно вместить в руку");
        // var overflownedoffset = (CardSize.CardScale.x / 2 ) / (_overflownHandSize - _fullHandSize)
        //  distribute cards
        FindMidOfTheHand();
        CardsPool = new GameObject[HandSize];
        Vector3 midCoordinateOfTheHand = new Vector3();
        for (int i = 0; i < HandSize; ++i)  //  creating card and placing it
        {
            Debug.Log($"next free position = {_nextFreePositionX} position y = {_positionY} distance between centers of cells = {_distanceBetweenCentersOfCards} \n full hand size = {_fullHandSize} overflown hand size = {_overflownHandSize}");
            CardsPool[i] = _cardFabric.Create(new Vector3(_nextFreePositionX, _positionY, _handCorners[0].z));
            if (CardSize.CardScale.x * i * (OffsetBetweenCards - 1) + RightLeftShift * 2 > _handCorners[3].x - _handCorners[0].x)
            {
                OffsetBetweenCards = (i - _fullHandSize ) * (CardSize.CardScale.x / 2) / (_overflownHandSize - _fullHandSize);
                _nextFreePositionX -= (CardSize.CardScale.x / 2) / (_overflownHandSize - _fullHandSize);
            }
            else
            {
                _nextFreePositionX += _distanceBetweenCentersOfCards;
            }
            Replace(midCoordinateOfTheHand.x - (i - 1) * _distanceBetweenCentersOfCards - _distanceBetweenCentersOfCards / 2 , i);
        }
    }

    //private float FindPlaceForCard() {
    //    //  проверка превышает ли размер спрайта умноженный на размер пула границы руки 
    //    return new float();
        
    //}

    private void FindMidOfTheHand() //  initializates 2 variable which represents coordinates in the middle of the hand area
    {
        _nextFreePositionX = _handCorners[0].x + (_handCorners[3].x - _handCorners[0].x) / 2;   //  origin + half of the area width -> coordinate of mid
        _positionY = _handCorners[1].y - _handCorners[0].y;  
    }

    private void Replace(float startingPoint, int amountOfCards) //  receive starting point on the x axis and put card every distancebetweencenters
    {
        for (int i = 0; i < amountOfCards; ++i)
        {
            CardsPool[i].transform.position = new Vector3 (startingPoint + _distanceBetweenCentersOfCards * i, CardsPool[i].transform.position.y, CardsPool[i].transform.position.z);
        }
    }

    //  realization
    private GameObject[] CardsPool;
    private Vector3[] _handCorners;
    private IFabric _cardFabric;
    //
    private float _nextFreePositionX = 0.0f;
    private float _positionY = 0.0f;
    private float _distanceBetweenCentersOfCards = 0.0f;
    private int _fullHandSize = 0;  // full-fledjed spacing
    private int _overflownHandSize = 0;
}