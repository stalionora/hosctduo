using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

////////////////////////////////////////
//  Pushes gameobject of dragged card(for figure distributor)
//  Dependent from drag handler, gameService, icellstrackerservice
////////////////////////////////////////
class FigureDataObserver : MonoBehaviour {

    public UnityEvent<GameObject> OnPushingData = new();
    
    private void Start(){
        _dragHandler = GetComponent<CardDragHandler>();
        _dragHandler.OnCardDragStart.AddListener(StartPushingOnDragEnd); //  pushing card data
        _dragHandler.OnCardDragEnd.AddListener(StopObserving);
    }
    public void StartPushingOnDragEnd(PointerEventData data) {
        GameService.GetService<ICellsTracker>().GetOnOutOfBorder().AddListener(StopPushingOnOutOfBorder);
        _dragHandler.OnCardDragEnd.AddListener(PushData);
    }
    public void StopPushingOnOutOfBorder() {
        StopObserving();
        GameService.GetService<ICellsTracker>().GetOnCellChange().AddListener(RestartObserving);
    }
    public void PushData(PointerEventData data)
    {
        OnPushingData.Invoke(this.transform.gameObject);    //  putting 
    }
    private void StopObserving(PointerEventData data = null) { 
        //_dragHandler.OnCardDragStart.RemoveListener(StartPushingOnDragEnd); //  pushing card data
        GameService.GetService<ICellsTracker>().GetOnOutOfBorder().RemoveListener(StopPushingOnOutOfBorder);
        _dragHandler.OnCardDragEnd.RemoveListener(PushData);
    }
    private void RestartObserving(Vector3 shit) {
        StartPushingOnDragEnd(null);
        GameService.GetService<ICellsTracker>().GetOnCellChange().RemoveListener(RestartObserving);
    }

    //  fields
    private CardDragHandler _dragHandler;
}