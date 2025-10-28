using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

////////////////////////////////////////
//  Pushes gameobject of dragged card(for figure distributor)
//  Dependent from drag handler, gameService, icellstrackerservice,figures movement way service
////////////////////////////////////////
class FigureDataObserver : MonoBehaviour {

    public UnityEvent<GameObject> OnPushingData = new();
    
    private void Start(){
        _dragHandler = GetComponent<CardDragHandler>();
    }
    public void ActivateObservingFunctioin(PointerEventData data) {
        GameService.GetService<ICellsTracker>().GetOnOutOfBorder().AddListener(StopPushingOnOutOfBorder);
        GameService.GetService<MovementWayService>().OnSettingTarget.AddListener(PushData);
    }
    public void PushData(PointerEventData data)
    {
        OnPushingData.Invoke(this.transform.gameObject);    //  putting 
    }
    public void DeactivateObservingFunction(PointerEventData data = null) { 
        //_dragHandler.OnCardDragStart.RemoveListener(ActivateObservingFunctioin); //  pushing card data
        GameService.GetService<ICellsTracker>().GetOnOutOfBorder().RemoveListener(StopPushingOnOutOfBorder);
        GameService.GetService<MovementWayService>().OnSettingTarget.RemoveListener(PushData);
    }
    private void RestartObserving(Vector3 shit) {
        ActivateObservingFunctioin(null);
        GameService.GetService<ICellsTracker>().GetOnCellChange().RemoveListener(RestartObserving);
    }
    private void StopPushingOnOutOfBorder() {    //  Stop on out of border
        DeactivateObservingFunction();
        GameService.GetService<ICellsTracker>().GetOnCellChange().AddListener(RestartObserving);
    }

    //  fields
    private CardDragHandler _dragHandler;
}