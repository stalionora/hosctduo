using UnityEngine;
public class Card: MonoBehaviour {
	public CardData Stats { get { return _stats; } }

	public void SetStats(CardData data) { 
		_stats = data;
	}

	private CardData _stats;
}