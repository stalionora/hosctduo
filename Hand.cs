using UnityEngine;
using UnityEngine.Rendering;

//  model which only contains cards from pool

public class Hand : MonoBehaviour{
	public GameObject[] Cards { get { return cards; } set { cards = value; } }

    private GameObject[] cards; //  pool's resources + current amount of cards
}
