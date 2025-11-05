using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
///////////////////////////////////////////////////////////
//  depends on: 
///////////////////////////////////////////////////////////
public class TimerMock : IService
{
    public UnityEvent OnEverySecond = new(); 
    public UnityEvent OnTurnEnd = new(); 
    public void Initialize()
    {
        _isRunning = true;
        RunTimer();
        MockEndTurn();
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private async void RunTimer()
    {
        while (_isRunning) { 
            await Task.Delay(1000); 
            OnEverySecond.Invoke();         
        }
    }
    private async void MockEndTurn()
    {
        while (_isRunning)
        {
            await Task.Delay((int)(_turnTime)); 
            OnTurnEnd.Invoke(); //  null reference при уничтожении 
        }
    }
    private bool _isRunning;
    private float _turnTime = 2000f; //10^-3

}