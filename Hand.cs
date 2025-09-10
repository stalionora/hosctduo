using UnityEngine;

//  model which only contains cards from pool

class Hand : MonoBehaviour{
    public GameObject[] Cards { get { return cards; } set { cards = value; } }

    

    private GameObject[] cards; //  pool's resources + current amount of cards
}
