using Unity.VisualScripting;
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
    [SerializeField]
    private RectTransform Rect;
    //  determine scales and position of the hand
    public void Initialize() {  
        Debug.Log("HandDistributor initialization");
        //this.HandScale = GetComponent<HandScale>();
        //  invariant
        if (this.HandScale.CardSize == null)
            Debug.Log("SerializeFields was not assigned");
            //  is used for starting point to add distances
        //  total area size
        _activeAreaSize = Rect.rect.width;
        Debug.Log("rect.width " + _activeAreaSize);

        //  determination of the card amount related bounds
        //  -s- ... -c- o -c- o -c- ... -s-     -->     ... -c- o -c- ...
        _fullHandSize = (int)((_activeAreaSize - this.HandScale.LeftShift * 2) / (this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards));  // 
        if (this.HandScale.CardSize.CardScale.x / 2 * this.HandScale.StartingHandSize + this.HandScale.LeftShift * 2 > _activeAreaSize)    // interrupt, if number of cards is greater than length of area / half card width
            Debug.Log("Больше карт чем можно вместить в руку");

        //

        //  finding mid of the hand
        _midCoordinateOfTheHand.x = Rect.rect.xMin + Rect.rect.width / 2;   //  origin + half of the area width -> coordinate of mid
        _midCoordinateOfTheHand.y = Rect.rect.yMin + HandScale.CardSize.CardScale.y / 2 + Mathf.Abs(Rect.rect.height - HandScale.CardSize.CardScale.y) / 2;


    }

    //  cards shouldnt be smaller then hand size 
    public void DistributeCards(GameObject[] cards) {
        if (cards.Length < this.HandScale.StartingHandSize){
            Debug.Log($"wrong size of cards distributor");
            return;
        }
        if (cards == null) {
            Debug.Log("There is no cards. NO CARDS");
        }
        _distanceBetweenCentersOfCards = this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards;
        //float sizeDifference = 0.0f;
        //  distribute cards
        for (int i = 0; i < this.HandScale.StartingHandSize; ++i){  //  creating card and placing it
            // ????????????????
            cards[i].transform.SetParent(transform, false);
            cards[i].SetActive(true);
            cards[i].name = $"Card#{i}";
            //Replace(_midCoordinateOfTheHand.x - (i / 2) * _distanceBetweenCentersOfCards - CardSize.CardScale.x / 2 * (i % 2) + CardSize.CardScale.x / 2 - OffsetBetweenCards / 2 * ((i + 1) % 2), i);
            //  for the different 
            //if (((this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards) * i - this.HandScale.OffsetBetweenCards + this.HandScale.LeftShift * 2) > _activeAreaSize)
            if(i > _fullHandSize){
                //sizeDifference = i * (this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards) - this.HandScale.OffsetBetweenCards - _activeAreaSize - 2 * this.HandScale.LeftShift;
                Debug.Log("Too muh cards in the hand");
                //  ... - card - c/2 - card - c/2 - card - c/2 - ...
                //_distanceBetweenCentersOfCards = this.HandScale.CardSize.CardScale.x + this.HandScale.OffsetBetweenCards - sizeDifference / i - this.HandScale.CardSize.CardScale.x;
                _distanceBetweenCentersOfCards = (_activeAreaSize - this.HandScale.CardSize.CardScale.x - 2 * this.HandScale.LeftShift) / i;
            }

            Replace(cards, _midCoordinateOfTheHand.x - ((_distanceBetweenCentersOfCards * (float)i)/2), i); //  starting point is left border of card?
            //Replace(cards, _midCoordinateOfTheHand.x - ((i + 1) * this.HandScale.CardSize.CardScale.x + (i * this.HandScale.OffsetBetweenCards))/2 + this.HandScale.CardSize.CardScale.x / 2, i);
        }
        //Debug.Log($"position y = {_midCoordinateOfTheHand.y} \t full hand size = {_fullHandSize} \t overflown hand size = {_overflownHandSize}");
    }

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
}