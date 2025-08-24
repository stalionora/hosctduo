using UnityEngine;
//////////////////////////
//
//  PROVIDES CARDS,
//  Dependent from hand, CardSize
//
//////////////////////////
public class HandDistributor : MonoBehaviour {
    [SerializeField]
    private HandScale HandScale;
    //  determine scales and position of the hand
    public void Initialize() {  
        Debug.Log("HandDistributor initialization");
        //this.HandScale = GetComponent<HandScale>();
        //  invariant
        if (this.HandScale.CardSize == null)
            Debug.Log("SerializeFields was not assigned");
            //  is used for starting point to add distances
        var rect = GetComponent<RectTransform>();
       
        //  total area size
        _activeAreaSize = rect.rect.width;
        Debug.Log("rect.width " + _activeAreaSize);

        //  determination of the card amount related bounds
        //  -s- ... -c- o -c- o -c- ... -s-     -->     ... -c- o -c- ...
        _overflownHandSize = (int)((_activeAreaSize - this.HandScale.LeftShift * 2) / (this.HandScale.CardSize.CardScale.x  / 2));
        _fullHandSize = (int)((_activeAreaSize - this.HandScale.LeftShift * 2 + this.HandScale.OffsetBetweenCards) / (this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards));  // 
        if (this.HandScale.CardSize.CardScale.x / 2 * this.HandScale.HandSize + this.HandScale.LeftShift * 2 > _activeAreaSize)    // interrupt, if number of cards is greater than length of area / half card width
            Debug.Log("Больше карт чем можно вместить в руку");

        //
        _distanceBetweenCentersOfCards = this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards;

        //  finding mid of the hand
        //_midCoordinateOfTheHand.x = (rect.rect.xMin + _activeAreaSize) / 2;   //  origin + half of the area width -> coordinate of mid
        _midCoordinateOfTheHand.x = rect.rect.xMin + rect.rect.width / 2;   //  origin + half of the area width -> coordinate of mid
        _midCoordinateOfTheHand.y = rect.rect.yMin + rect.rect.height / 2; 

    }

    //  cards shouldnt be smaller then hand size 
    public void DistributeCards(GameObject[] cards) {
        if (cards.Length < this.HandScale.HandSize){
            Debug.Log($"wrong size of cards distributor");
            return;
        }
        if (cards == null) {
            Debug.Log("There is no cards. NO CARDS");
        }

        //  distribute cards
        for (int i = 0; i < this.HandScale.HandSize; ++i)  //  creating card and placing it
        {
            // ????????????????
            cards[i].transform.SetParent(transform, false);
            cards[i].name = $"Card#{i}";
            Replace(cards, _midCoordinateOfTheHand.x - ((i + 1) * this.HandScale.CardSize.CardScale.x + (i * this.HandScale.OffsetBetweenCards))/2 + this.HandScale.CardSize.CardScale.x / 2, i);
            //Replace(_midCoordinateOfTheHand.x - (i / 2) * _distanceBetweenCentersOfCards - CardSize.CardScale.x / 2 * (i % 2) + CardSize.CardScale.x / 2 - OffsetBetweenCards / 2 * ((i + 1) % 2), i);
            //  for the different 
            if ((this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards) * i - this.HandScale.OffsetBetweenCards + this.HandScale.LeftShift * 2 > _activeAreaSize)
            {
                Debug.Log("shit");
                //  ... - card - c/2 - card - c/2 - card - c/2 - ...
                _distanceBetweenCentersOfCards -= (this.HandScale.CardSize.CardScale.x / 2 + this.HandScale.OffsetBetweenCards) / (this.HandScale.HandSize - _fullHandSize);
            }
        }
        Debug.Log($"position y = {_midCoordinateOfTheHand.y} \t full hand size = {_fullHandSize} \t overflown hand size = {_overflownHandSize}");
        //Replace(_midCoordinateOfTheHand.x - (HandSize - 1) * (_distanceBetweenCentersOfCards)/2f, HandSize);
    }

    //public void Initialize()
    //{
    //    Debug.Log("HandDistributor initialization");

    //    //  invariant
    //    if (CardSize == null || _cardFabric == null)
    //        Debug.Log("SerializeFields was not assigned");

    //    //  is used for starting point to add distances
    //    var rect = GetComponent<RectTransform>();
    //    _handCorners = new Vector3[4];
    //    for (int i = 0; i < 4; ++i)
    //    {
    //        _handCorners[i] = new Vector3();    //  neccessary,  nulls
    //    }

    //    //  total area size
    //    scaleFactor = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.x; //
    //    rect.GetWorldCorners(_handCorners);
    //    for (int i = 0; i < 4; ++i)
    //    {
    //        _handCorners[i] /= scaleFactor;    //  scaling
    //    }
    //    _activeAreaSize = Mathf.Abs(_handCorners[3].x - _handCorners[0].x);
    //    Debug.Log("size.delta: " + rect.sizeDelta + ", min: " + rect.anchorMin + ", max: " + rect.anchorMax);
    //    Debug.Log(_activeAreaSize);


    //    //  determination of the card amount related bounds
    //    //  -s- ... -c- o -c- o -c- ... -s-     -->     ... -c- o -c- ...
    //    _overflownHandSize = (int)((_activeAreaSize - LeftShift * 2) / (CardSize.CardScale.x / 2));
    //    _fullHandSize = (int)((_activeAreaSize - LeftShift * 2 + OffsetBetweenCards) / (CardSize.CardScale.x + OffsetBetweenCards));  // 
    //    if (CardSize.CardScale.x / 2 * HandSize + LeftShift * 2 > _activeAreaSize)    // interrupt, if number of cards is greater than length of area / half card width
    //        Debug.Log("Больше карт чем можно вместить в руку");

    //    //
    //    _distanceBetweenCentersOfCards = CardSize.CardScale.x + OffsetBetweenCards;

    //    //  finding mid of the hand
    //    _midCoordinateOfTheHand.x = _handCorners[0].x + (_activeAreaSize) / 2;   //  origin + half of the area width -> coordinate of mid
    //    _nextFreePositionX = _midCoordinateOfTheHand.x;
    //    _positionY = (_handCorners[0].y + (_handCorners[1].y - _handCorners[0].y) / 2);

    //    //  distribute cards
    //    CardPool = new GameObject[HandSize];
    //    for (int i = 0; i < HandSize; ++i)  //  creating card and placing it
    //    {
    //        // ????????????????
    //        CardPool[i] = _cardFabric.Create(new Vector3(_nextFreePositionX, _positionY, _handCorners[0].z));
    //        CardPool[i].transform.SetParent(this.transform, false);    //  necessary
    //        CardPool[i].name = $"Card#{i}";
    //        if (i > 0)
    //            Replace(_midCoordinateOfTheHand.x - (i - 1) * (_distanceBetweenCentersOfCards)/2f, i);
    //        //Replace(_midCoordinateOfTheHand.x - (i / 2) * _distanceBetweenCentersOfCards - CardSize.CardScale.x / 2 * (i % 2) + CardSize.CardScale.x / 2 - OffsetBetweenCards / 2 * ((i + 1) % 2), i);
    //        //  for the different 
    //        _nextFreePositionX += _distanceBetweenCentersOfCards;
    //        if ((CardSize.CardScale.x + OffsetBetweenCards) * i - OffsetBetweenCards + LeftShift * 2 > _activeAreaSize)
    //        {
    //            Debug.Log("shit");
    //            //  ... - card - c/2 - card - c/2 - ...
    //            _distanceBetweenCentersOfCards -= (CardSize.CardScale.x / 2 + OffsetBetweenCards) / (HandSize - _fullHandSize);
    //        }
    //        Debug.Log($"next free position = {_nextFreePositionX} position y = {_positionY} distance between centers of cells = {_distanceBetweenCentersOfCards} \n full hand size = {_fullHandSize} overflown hand size = {_overflownHandSize}");
    //        Debug.Log($"active area size = {_activeAreaSize} == " + $" {Vector3.Distance(_handCorners[3], _handCorners[0])}");
    //    }
    //    Replace(_midCoordinateOfTheHand.x - (HandSize - 1) * (_distanceBetweenCentersOfCards)/2f, HandSize);
    //}

    private void Replace(GameObject[] cards, float startingPoint, int amountOfCards) { //  receive starting point on the x axis and put card every distancebetweencenters    
        for (int i = 0; i < amountOfCards + 1; ++i){
            cards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPoint + _distanceBetweenCentersOfCards * i, _midCoordinateOfTheHand.y);   //  isnt changing local coordinates?
            //Debug.Log($"card #{i + 1}; starting point = {startingPoint}; position of card = {cards[i].GetComponent<RectTransform>().anchoredPosition.x}");
        }
    }

    //  realization
    private Vector3 _midCoordinateOfTheHand;
    //
    private float _distanceBetweenCentersOfCards = 0.0f;
    private float _activeAreaSize = 0.0f;
    private int _fullHandSize = 0;  //  full-fledjed spacing ???
    private int _overflownHandSize = 0; //  ???
}