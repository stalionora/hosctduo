using UnityEngine.Events;

public class Player {  
    public int _resources = 1;
    public int _healthPoints = 50; 
    public string _playersId;
    public string _playersName; 
}

public class PlayerController:ITurnBaseLogic {
    public UnityEvent<Player> OnUpdateStats = new();
    public Player Stats { get { return _player; } }
    public void RecalculateHealth(int value) {
        _player._healthPoints -= value;
        OnUpdateStats.Invoke(_player);
    }

    public void ActionOfTakingDamage(int value){
        RecalculateHealth(value);
    }

    public void AddPeople(int value) {
        _player._resources += value; 
        OnUpdateStats.Invoke(_player);
    }

    public void PerformOnTurnEnd(){
        AddPeople(1);
    }

    private Player _player = new Player();
}