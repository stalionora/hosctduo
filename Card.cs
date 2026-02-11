using UnityEngine;
public class Card: MonoBehaviour {
	public void SetStats(CardData data) { 
		_stats = data;
	}

	private CardData _stats;
}