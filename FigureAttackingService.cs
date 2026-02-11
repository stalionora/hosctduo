using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

class FigureAttackingService: IService, ITurnBaseLogic {
    public UnityEvent<GameObject> OnAttackOfPlayer = new();
    public UnityEvent<GameObject> OnAttackOfEnemy = new();
    public UnityEvent<Figure, Figure> OnClash = new();
    
    public void SetAttacker(Figure figure) { //  Listens ivent on attack ?from movement service OnEndOfWay
        _attackers.Add(figure);
        Debug.Log("ATTACKER HAVE BEEN ADDED");
    }

    public void RemoveAttacker() { }

    public void Clash(GameObject figureA, GameObject figureB) { //  disposable action
        
    }

    public void Initialize(){
        _enemyBase = GameObject.Find("EnemiesArea").GetComponent<Transform>().position;
        _playerBase = GameObject.Find("PlayersHand").GetComponent<Transform>().position;
        _enemyBase.x = 0;
        _playerBase.x = 0;
        
    }

    private void MoveFigureToAttack(Figure figure) {
        
    }

    public void PerformOnTurnEnd() {
        for (int i = 0; i < _attackers.Count; ++i) {
            if (_attackers[i] != null){
                _attackers[i].OnAttackingBase();
            }
            else
                _attackers.RemoveAt(i);
        }
    }

    private List<Figure> _attackers = new List<Figure>();
    private Vector3 _enemyBase;
    private Vector3 _playerBase;
}