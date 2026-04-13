using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//  dependent from IMecAction, CardType

public class CardDataImpl{
    public string Hash { get { return _hash; } }
	public int PeopleWorth { set { _peopleWorth = value; } get { return _peopleWorth; } }
    public CardType Traits { get { return _traits; } }
    public List<IMecTrigger> CardMechanicTriggers { get { return _cardsMechanicTriggers; } }
    public List<IMecTrigger> FigureMechanicTriggers { get { return _figureMechanicTriggers; } }

    public CardDataImpl(string hash, CardType cardTraits, int peopleWorth, IMecTrigger[] cardMechTriggers, IMecTrigger[] figureMechTriggers) {
        _traits = cardTraits;
        _hash = hash;
        _peopleWorth = peopleWorth;
        _figureMechanicTriggers = new List<IMecTrigger>();
        _cardsMechanicTriggers = new List<IMecTrigger>();
        if(cardMechTriggers != null)
            foreach (var trigger in cardMechTriggers)
                _cardsMechanicTriggers.Add(trigger);
        if(figureMechTriggers != null)
            foreach (var trigger in figureMechTriggers)
                _figureMechanicTriggers.Add(trigger);
    }

    public CardDataImpl(CardDataImpl copy) {
        _traits = copy._traits;
        _hash = copy._hash;
        _peopleWorth = copy._peopleWorth;
        _cardsMechanicTriggers = new List<IMecTrigger>();
        _figureMechanicTriggers = new List<IMecTrigger>();
        if (copy.CardMechanicTriggers != null)
            foreach (var mech in copy.CardMechanicTriggers)
                _cardsMechanicTriggers.Add(mech);
        if (copy.FigureMechanicTriggers != null)
            foreach (var mech in copy.FigureMechanicTriggers)
                _figureMechanicTriggers.Add(mech);
    }

    public void OnCardStatsReceiving(int peopleWorth) {
        _peopleWorth = peopleWorth;
	}

    //
    private CardType _traits;
	private string _hash;
	private int _peopleWorth;
    private List<IMecTrigger> _cardsMechanicTriggers;
    private List<IMecTrigger> _figureMechanicTriggers;
    //private int _cost;
}
