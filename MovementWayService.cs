using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/////////////////////////////////////////////////////////////////////////////////
//  singleton, dependends on: MovementWay, EventInpuTrigger, FigureDataObserver, FigureWayService
/////////////////////////////////////////////////////////////////////////////////
public class MovementWayService: IService{
    public UnityEvent<PointerEventData> OnSettingTarget = new();
    
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
        Debug.Log("start making wayway");
        _eventTrigger.gameObject.SetActive(true);
        _object.SetActive(true);
        _movementWay.StartMakingWay();
        _eventTrigger.OnCursorMoveCustom.AddListener(UpdateMovementWay);
        _eventTrigger.OnClickCustom.AddListener(OnStopMakingWay);
        _cardPosition = _cellsTracker.GetCurrentCellCoordinates();
        PerformCreationFlagSet();
    }

    public void OnStopMakingWay(PointerEventData data = null){
        Debug.Log("stop making way");
        if (_actOnEndflag) {    //  on the correct exit with click at enemy
            data.position = _cardPosition;
            OnSettingTarget.Invoke(data);
            GameService.GetService<FigureMovementService>().AddMovementWay(_movementWay.Points.ToArray());
            
        }
        _movementWay.Reset();
        _object.SetActive(false);
        _eventTrigger.gameObject.SetActive(false);
        _eventTrigger.OnCursorMoveCustom.RemoveListener(UpdateMovementWay);
        _eventTrigger.OnClickCustom.RemoveListener(OnStopMakingWay);
    }

    public void CancelMakingWay() { 
        _actOnEndflag = false;
        OnStopMakingWay();
    }

    public void CancelCreationFlagSet() { 
        _actOnEndflag = false;
    }
    public void PerformCreationFlagSet() { 
        _actOnEndflag = true;
    }
    private void UpdateMovementWay(Vector3 eventData){
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
//    public UnityEvent OnSettingTarget;

//    public static GameObject GetInstance() {
//        return _instance;
//    }

//    void Start() {
//        _eventTrigger = GameObject.Find("IndependentCellsTrackerTrigger").GetComponent<IndependentCellsTrackerTrigger>();
//        _movementWay = GetComponent<MovementWay>();
//        _eventTrigger.gameObject.SetActive(false);
//        _movementWay.gameObject.SetActive(false);
//        _instance = this.transform.gameObject;
//    }
    
//    public void StartMakingWay(GameObject draggedCard) {
//        _eventTrigger.gameObject.SetActive(true);
//        _movementWay.gameObject.SetActive(true);
//        _movementWay.StartMakingWay(ref draggedCard);
//        _eventTrigger.OnClickCustom.AddListener(OnStopMakingWay);
//    }

//    public void OnStopMakingWay(PointerEventData data){
//        _movementWay.Reset();    
//        _movementWay.gameObject.SetActive(false);
//        _eventTrigger.gameObject.SetActive(false);
//    }
//    static GameObject _instance;
//    IndependentCellsTrackerTrigger _eventTrigger;
//    MovementWay _movementWay;
//}