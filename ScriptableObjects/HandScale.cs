using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "HandScale", menuName = "ScriptableObject/HandScale")]
public class HandScale: ScriptableObject {
    //  interface
    [SerializeField]
    private CardSize _cardSize;
    [SerializeField]
    private int _startingCardsAmount;
    [SerializeField]
    private int _maximalDeckSize;
    [SerializeField]
    private float _offsetBetweenCards;
    [SerializeField]
    private float _leftShift;

	public CardSize CardSize{ get {return _cardSize; } }
    public int StartingHandSize { get {return _startingCardsAmount;} }
    public int MaximalDeckSize { get { return _maximalDeckSize; } }
	public float OffsetBetweenCards { get {return _offsetBetweenCards; } }
    public float LeftShift { get {return _leftShift; } }
}