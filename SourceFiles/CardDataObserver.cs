//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;

//////////////////////////////////////////
////  Pushes gameobject of dragged card(for figure distributor), set listener PUSHING DATA of MOVEMENT WAY SERVICE, calls listeners of PUSHING DATA
////  - COULD contain logic for interrupting plotting the movement way 
////  Dependent from drag handler, gameService, icellstrackerservice, figures movement way service
//////////////////////////////////////////
//public class CardDataObserver : MonoBehaviour {

//    public UnityEvent<GameObject> OnPushingData = new();
    
//    private void Start(){
//        _dragHandler = GetComponent<CardDragHandler>();
//    }
//    //public void ActivateCardObserver(PointerEventData data) {
        
//        //GameService.GetService<MovementWayService>().OnSettingTarget.AddListener(PushData);
//        //GameService.GetService<ICellsTracker>().GetOnReturnInBorder().AddListener(StopPushingOnOutOfBorder);    //  first move in the field
//        //GameService.GetService<ICellsTracker>().GetOnOutOfBorder().AddListener(DeactivateObservingFunction);
//    //}
//    public void DeactivateCardObserver(PointerEventData uselessData) {  //  for extern usage
//        DeactivateObservingFunction();
//    }
//    private void PushData(PointerEventData data){
//        Debug.Log("PUSHING CARD IMAGE");
//        OnPushingData.Invoke(this.transform.gameObject);    //  putting 
//    }
//    private void DeactivateObservingFunction(){
//        GameService.GetService<ICellsTracker>().GetOnOutOfBorder().RemoveListener(DeactivateObservingFunction);
//        GameService.GetService<MovementWayService>().OnSettingTarget.RemoveListener(PushData);
//    }
//    //private void StopPushingOnOutOfBorder() {    //  Stop on out of border
//    //    GameService.GetService<ICellsTracker>().GetOnReturnInBorder().RemoveListener(StopPushingOnOutOfBorder);
//    //    GameService.GetService<ICellsTracker>().GetOnOutOfBorder().AddListener(DeactivateObservingFunction);
//    //}

//    //  fields
//    private CardDragHandler _dragHandler;
//}