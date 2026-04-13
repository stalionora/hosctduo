using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//  dependent from IMecAction, Timer

public class DeferredFigureActionTrigger : IMecTrigger{
    public DeferredFigureActionTrigger(int turnsAmount, IFigureMecAction[] mechanics){
        _mechanic = new Dictionary<IFigureMecAction, int>();
        _currentMec = null;
        _context = new();
        _dependencies = null;
        if (mechanics.Length > 0 && turnsAmount > 0)
            foreach (var mechanic in mechanics){
                _mechanic.Add(mechanic, turnsAmount);
            }
        else
            Debug.LogWarning("Cast time for DeferredFigureActionTrigger should be at least 1s");
        Debug.Log($"Creation of DeferredFigureActionTrigger with {turnsAmount} turns to cast and {mechanics.Length} mechanics... HASH #{_mechanic.GetHashCode()}");
    }

    public void StartHandling(FigureContext figureContext, ref FigureFuncDependencyContext dependencies){
        Debug.Log($"StartHandling: trigger hash #{this.GetHashCode()}\tmechanics hash #{_mechanic.GetHashCode()}");
        if (figureContext.CurrentFigure == null)
            Debug.LogError("There is no game object in the mechanic handler");
        else{
            _context = figureContext;
            _dependencies = dependencies;
            GameService.GetService<TimerMock>().OnTurnEnd.AddListener(OnEveryTurn);
        }
    }

    public void StopHandling(){
        Debug.Log("Stopping of figure mechanic handling...");
        GameService.GetService<TimerMock>().OnTurnEnd.RemoveListener(OnEveryTurn);
    }

    private void StopHandling(IFigureMecAction obj) { 
        Debug.Log($"Stopping of handling {obj.ToString()}...");
        _mechanic.Remove(obj);
    }

    private void OnEveryTurn() {
        for(int i = 0; i < _mechanic.Keys.Count; ++i){
            _currentMec = _mechanic.ElementAt(i).Key;
            Debug.Log($"-------------------------------------------------------------------------------");
            Debug.Log($"Deferred mechanic: {_currentMec.ToString()}, {_mechanic[_currentMec]} turns before executing ");
            _mechanic[_mechanic.ElementAt(i).Key] -= 1;
            if (_mechanic[_currentMec] <= 0){
                _currentMec.Execute(_context, ref _dependencies);
                StopHandling(_currentMec);
            }
        }
    }

    //////////////////////  not implemented interface with context's point
    public void StartHandling(CardContext cardContext){
        throw new System.NotImplementedException();
    }

    //
    private IFigureMecAction _currentMec;
    //  mechanic + turns to cast
    private Dictionary<IFigureMecAction, int> _mechanic;
    private FigureContext _context;
    private FigureFuncDependencyContext _dependencies;
}
