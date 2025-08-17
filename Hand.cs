using UnityEngine;

public class Hand: MonoBehaviour {
    //  interface
    [SerializeField]
    private CardSize _cardSize;
    [SerializeField]
    private int _size;
    [SerializeField]
    private float _offsetBetweenCards;
    [SerializeField]
    private float _leftShift;

    public CardSize CardSize{ get {return _cardSize; } }
    public int HandSize { get {return _size;} }
    public float OffsetBetweenCards { get {return _offsetBetweenCards; } }
    public float LeftShift { get {return _leftShift; } }
    //
    //public Vector3 MidCoordinate {  get; }
    //public float ActiveAreaSize { get; }
    //public int OverflownSize { get; }
    //public int FullSize { get; }
    //public float DistanceBetweenCentersOfCards { get; };
    //public void Initialize() {
    //    Debug.Log("HandDistributor initialization");

    //    //  invariant
    //    if (CardSize == null)
    //        Debug.Log("SerializeFields was not assigned");
    //    //  is used for starting point to add distances
    //    var rect = GetComponent<RectTransform>();

    //    //  total area size
    //    _activeAreaSize = rect.rect.width;
    //    Debug.Log("rect.width " + _activeAreaSize);

    //    //  determination of the card amount related bounds
    //    //  -s- ... -c- o -c- o -c- ... -s-     -->     ... -c- o -c- ...
    //    _overflownSize = (int)((_activeAreaSize - LeftShift * 2) / (CardSize.CardScale.x / 2));
    //    _fullSize = (int)((_activeAreaSize - LeftShift * 2 + OffsetBetweenCards) / (CardSize.CardScale.x + OffsetBetweenCards));  // 
    //    if (CardSize.CardScale.x / 2 * HandSize + LeftShift * 2 > _activeAreaSize)    // interrupt, if number of cards is greater than length of area / half card width
    //        Debug.Log("Больше карт чем можно вместить в руку");

    //    //
    //    _distanceBetweenCentersOfCards = CardSize.CardScale.x + OffsetBetweenCards;

    //    //  finding mid of the hand
    //    //_midCoordinateOfTheHand.x = (rect.rect.xMin + _activeAreaSize) / 2;   //  origin + half of the area width -> coordinate of mid
    //    _midCoordinate.x = rect.rect.xMin + rect.rect.width / 2;   //  origin + half of the area width -> coordinate of mid
    //    _midCoordinate.y = rect.rect.yMin + rect.rect.height / 2;

    //}

    ////  
    //private Vector3 _midCoordinate = new();
    
    ////  realization
    //private float _activeAreaSize = 0;
    //private int _overflownSize = 0;
    //private int _fullSize = 0;
    //private float _distanceBetweenCentersOfCards = 0;
}