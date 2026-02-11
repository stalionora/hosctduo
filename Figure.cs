using UnityEngine;


public class Figure : MonoBehaviour {
    public BaseAttackSetter AttackSetter { get { return AttackSetter; } }

    public void SetSide(BaseAttackSetter attackFunc) {
        _attackFunc = attackFunc;
        _casualAnimatorController = GetComponent<FigureAnimatorController>();
    }

    public void SetStats(CardData statsData){ 
        _model = statsData;
	}

    public void SetRoute(){

    }

    public void OnAttackingBase() {
        Debug.Log("ATTACKIN BASE");
        _casualAnimatorController.PlayAttackAnimation();
    }

    public void AttackBase() {
        _attackFunc.Attack(_model.PeopleWorth);
        _casualAnimatorController.PlayReturnAnimation();
    }

    public void Fight(int incomingDamage){    //  retunrs value of attack
        _model.PeopleWorth -= incomingDamage;
    }

    public void Idle() {
        _casualAnimatorController.PlayIdleAnimation();
    }

    // should contains route or array of points to calculate it
    // stats from scriptable object
    private CardData _model;
    private GameObject _gameObject;
    private FigureAnimatorController _casualAnimatorController;
    //private Animator _animator;

    //  command
    private BaseAttackSetter _attackFunc;

    private int buf = 0;
}


public class FigureData{
    public int PeopleWorth{ set { _peopleWorth = value; } get { return _peopleWorth;} }
    private int _peopleWorth = 1;  
}
