using NUnit.Framework;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/////////////////////////////////////////////////////////////////////////////////
//  singleton, dependends on: MovementWay, EventInputTrigger, FigureWayService, cellsTrackerService
/////////////////////////////////////////////////////////////////////////////////
public class MovementWayService: IService{
    public UnityEvent<PointerEventData> OnSettingTarget = new();
    public UnityEvent OnInterruptingMovementSetting = new();
    
    public static GameObject GetInstance() {
        return _object;
    }
    
    public void Initialize() {
        _eventTrigger = GameObject.Find("IndependentCellsTrackerTrigger").GetComponent<IndependentCellsTrackerTrigger>();
        _cellsTracker = GameService.GetService<ICellsTracker>();
        _object = new GameObject();
        _movementWay = _object.AddComponent<MovementWay>();
        _mouseTracker = new MouseTracker(new Vector3(0,0, _zPointOfCellsMatrix));
        _eventTrigger.gameObject.SetActive(false);
        _object.SetActive(false);
    }
    
    public void StartMakingWay(PointerEventData data) {
        Debug.Log("start making way");
        _movementWay.StartMakingWay();
        _object.SetActive(true);
        _eventTrigger.gameObject.SetActive(true);
        _cardPosition = _cellsTracker.GetCurrentCellCoordinates();
        PerformCreationFlagSet();
        _cellsTracker.GetOnCellChange().AddListener(_movementWay.AddPoint);
        _eventTrigger.OnCursorMoveCustom.AddListener(UpdateMovementWay);
        _eventTrigger.OnClickCustom.AddListener(OnStopMakingWay);
    }

    public void OnStopMakingWay(PointerEventData data = null){
        _cellsTracker.GetOnCellChange().RemoveListener(_movementWay.AddPoint);
        Debug.Log("stop making way");
        if (_actOnEndflag) {    //  on the correct exit with click at enemy
            data.position = _cardPosition;
            OnSettingTarget.Invoke(data);
            foreach(var shitchen in _movementWay.Points)
                Debug.Log(shitchen);
            GameService.GetService<FigureMovementService>().AddMovementWay(_movementWay.Points.ToArray());
            
        }
        _movementWay.Reset();
        _object.SetActive(false);
        _eventTrigger.gameObject.SetActive(false);
        _eventTrigger.OnCursorMoveCustom.RemoveListener(UpdateMovementWay);
        _eventTrigger.OnClickCustom.RemoveListener(OnStopMakingWay);
        _cellsTracker.GetOnOutOfBorder().RemoveListener(CancelMakingWay); 
    }

    public void CancelMakingWay() { 
        _actOnEndflag = false;
        OnStopMakingWay();
        OnInterruptingMovementSetting.Invoke();
    }

    public void CancelCreationFlagSet() { 
        _actOnEndflag = false;
    }
    public void PerformCreationFlagSet() { 
        _actOnEndflag = true;
    }
    private void UpdateMovementWay(Vector3 eventData){  //  Movement way should `have started to listen cells tracker on this call
        //Debug.Log("update way");
        _cellsTracker.CalcuateCurrentCell(_mouseTracker.TrackMouse(eventData));
    }


    //
    static private GameObject _object;
    
    private ICellsTracker _cellsTracker;
    
    private IndependentCellsTrackerTrigger _eventTrigger;
    private MouseTracker _mouseTracker;
    private MovementWay _movementWay;
    private Vector3 _cardPosition = new();
 
    private const float _zPointOfCellsMatrix = 90f;
    private bool _actOnEndflag = false;
}