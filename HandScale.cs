using UnityEngine;

[CreateAssetMenu(fileName = "HandScale", menuName = "ScriptableObject/HandScale")]
public class HandScale: ScriptableObject {
    //  interface
    [SerializeField]
    private CardSize _cardSize;
    [SerializeField]
    private int _startingCardsAmount;
    [SerializeField]
    private float _offsetBetweenCards;
    [SerializeField]
    private float _leftShift;

    public CardSize CardSize{ get {return _cardSize; } }
    public int HandSize { get {return _startingCardsAmount;} }
    public float OffsetBetweenCards { get {return _offsetBetweenCards; } }
    public float LeftShift { get {return _leftShift; } }
}