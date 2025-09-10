using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

////////////////////////////////////////
//  Dependent from drag handler, gameService, icellstrackerservice
////////////////////////////////////////
class FigureDataObserver : MonoBehaviour {

    public UnityEvent<GameObject> OnPushingData = new();

    public void StartObserving(PointerEventData data) {
        var dragHandler = GetComponent<CardDragHandler>();
        dragHandler.OnCardDragEnd.AddListener(PushData);
    }
    public void StopObserving() {
        var dragHandler = GetComponent<CardDragHandler>();
        dragHandler.OnCardDragEnd.RemoveListener(PushData);
        GameService.GetService<ICellsTracker>().GetOnCellChange().AddListener(RestartObserving);
    }
    private void RestartObserving(Vector3 shit) {
        StartObserving(null);
        GameService.GetService<ICellsTracker>().GetOnCellChange().RemoveListener(RestartObserving);
    }
    public void PushData(PointerEventData data)
    {
        OnPushingData.Invoke(this.transform.gameObject);    //  putting 
    }
}