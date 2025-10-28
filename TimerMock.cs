using System.Threading.Tasks;
using UnityEngine;

public class TimerMock : IService
{
    private FigureMovementService _figureService;
    private bool _isRunning;
    private float _turnTime = 10f;

    public void Initialize()
    {
        _figureService = GameService.GetService<FigureMovementService>();
        _isRunning = true;
        RunTimer();
    }

    private async void RunTimer()
    {
        while (_isRunning)
        {
            await Task.Delay((int)(_turnTime * 1000)); // миллисекунды
            _figureService.PerformOnTurnEnd();
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }
}