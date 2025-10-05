using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/////////////////////////////////////////////////////////////////////////////////
//  singleton, dependends on: MovementWay, EventInpuTrigger, FigureDataObserver
/////////////////////////////////////////////////////////////////////////////////
class MovementWayService: IService{
    public UnityEvent OnSettingTarget;

    public static GameObject GetInstance() {
        return _object;
    }

    public void Initialize() {
        _eventTrigger = GameObject.Find("EventInputTrigger").GetComponent<EventInputTrigger>();
        _cellsTracker = GameService.GetService<ICellsTracker>();
        _object = new GameObject();
        _movementWay = _object.AddComponent<MovementWay>();
        _eventTrigger.gameObject.SetActive(false);
        _object.SetActive(false);
    }
    
    public void StartMakingWay(GameObject draggedCard) {
        Debug.Log("start making wayway");
        _eventTrigger.gameObject.SetActive(true);
        _object.SetActive(true);
        _movementWay.StartMakingWay(ref draggedCard);
        _eventTrigger.OnCursorMoveCustom.AddListener(UpdateMovementWay);
        _eventTrigger.OnClickCustom.AddListener(StopMakingWay);
    }

    public void StopMakingWay(PointerEventData data){
        Debug.Log("stop making way");
        _movementWay.Reset();    
        _object.SetActive(false);
        _eventTrigger.gameObject.SetActive(false);
        _eventTrigger.OnCursorMoveCustom.RemoveListener(UpdateMovementWay);
        _eventTrigger.OnClickCustom.RemoveListener(StopMakingWay);
    }

    void UpdateMovementWay(Vector3 eventData){
        //Debug.Log("update way");
        _cellsTracker.CalcuateCurrentCell(eventData);
    }


    static GameObject _object;
    ICellsTracker _cellsTracker;
    EventInputTrigger _eventTrigger;
    MovementWay _movementWay;
}
//    public UnityEvent OnSettingTarget;

//    public static GameObject GetInstance() {
//        return _instance;
//    }

//    void Start() {
//        _eventTrigger = GameObject.Find("EventInputTrigger").GetComponent<EventInputTrigger>();
//        _movementWay = GetComponent<MovementWay>();
//        _eventTrigger.gameObject.SetActive(false);
//        _movementWay.gameObject.SetActive(false);
//        _instance = this.transform.gameObject;
//    }
    
//    public void StartMakingWay(GameObject draggedCard) {
//        _eventTrigger.gameObject.SetActive(true);
//        _movementWay.gameObject.SetActive(true);
//        _movementWay.StartMakingWay(ref draggedCard);
//        _eventTrigger.OnClickCustom.AddListener(StopMakingWay);
//    }

//    public void StopMakingWay(PointerEventData data){
//        _movementWay.Reset();    
//        _movementWay.gameObject.SetActive(false);
//        _eventTrigger.gameObject.SetActive(false);
//    }
//    static GameObject _instance;
//    EventInputTrigger _eventTrigger;
//    MovementWay _movementWay;
//}