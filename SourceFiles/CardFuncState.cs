using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.WSA;

public abstract class CardFuncState{
    public abstract void Enter(ref CardContext cc, ref DependencyContext dc);
    public abstract void Exit(ref CardContext cc, ref DependencyContext dc);
    
}

public class DragDetectionState : CardFuncState {
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies) {
        Debug.Log("DRAG DETECTION STATE");
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {    //  can be optimized
        
    }
}

public class CardMovementState : CardFuncState {
    public override void Enter(ref CardContext cardContext,ref DependencyContext dependencies){
        //dependencies.PositionIndicator.WaitFirstEntrance(); //  turn on
        Debug.Log("CARD MOVEMENT STATE");
        cardContext.DragHandler.OnCardDrag.AddListener(dependencies.CardMovementService.OnDrag);
        cardContext.DragHandler.OnCardDragEnd.AddListener(dependencies.CardMovementService.OnEndDrag);
        dependencies.CellsTrackerService.GetOnOutOfBorder().AddListener(dependencies.CardMovementService.ReturnInHandOnEndDrag);
        dependencies.CellsTrackerService.GetOnReturnInBorder().AddListener(dependencies.CardMovementService.PlaceOnFieldOnEndDrag);
        dependencies.CellsTrackerService.GetOnCellChange().AddListener(dependencies.VisualShit.HighlightingElement);
        dependencies.CellsTrackerService.GetOnOutOfBorder().AddListener(dependencies.VisualShit.StopHighlightingLastElement);
        dependencies.CardMovementService.OnBeginDrag(cardContext.EventData);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {

        cardContext.DragHandler.OnCardDragEnd.RemoveListener(dependencies.CardMovementService.OnEndDrag);
        cardContext.DragHandler.OnCardDrag.RemoveListener(dependencies.CardMovementService.OnDrag);
        dependencies.CellsTrackerService.GetOnOutOfBorder().RemoveListener(dependencies.CardMovementService.ReturnInHandOnEndDrag);
        dependencies.CellsTrackerService.GetOnReturnInBorder().RemoveListener(dependencies.CardMovementService.PlaceOnFieldOnEndDrag);
        dependencies.CellsTrackerService.GetOnCellChange().RemoveListener(dependencies.VisualShit.HighlightingElement);
        dependencies.CellsTrackerService.GetOnOutOfBorder().RemoveListener(dependencies.VisualShit.StopHighlightingLastElement);
        dependencies.VisualShit.StopHighlightingLastElement();
        //dependencies.PositionIndicator.WaitActivationFromEvent();   //  hide indicator for the next cells tracker call 
    }
}

public class SettingMovementWayState : CardFuncState{
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies){
        Debug.Log("SETTING MOVEMENT WAY STATE");
        //dependencies.VisualShit.HighlightingElement();
        dependencies.CellsTrackerService.GetOnOutOfBorder().AddListener(dependencies.MovementWayService.CancelMakingWay);
        dependencies.MovementWayService.OnInterruptingMovementSetting.AddListener(dependencies.CardMovementService.ReturnLastCardInTheHand);
        dependencies.CellsTrackerService.GetOnCellChange().AddListener(dependencies.VisualShit.HighlightingElement);
        dependencies.MovementWayService.StartMakingWay(cardContext.EventData);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies){
        dependencies.CellsTrackerService.GetOnOutOfBorder().RemoveListener(dependencies.MovementWayService.CancelMakingWay);
        dependencies.MovementWayService.OnInterruptingMovementSetting.RemoveListener(dependencies.CardMovementService.ReturnLastCardInTheHand);
        //dependencies.PositionIndicator.DeactivateTracking();   //  turn off 
        dependencies.CellsTrackerService.GetOnCellChange().RemoveListener(dependencies.VisualShit.HighlightingElement);
        dependencies.VisualShit.StopHighlightingLastElement();
        
    }
}

//public class DataTransferState: CardFuncState { 
//    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies){
//        //_movementWayService.OnSettingTarget.AddListener(_positionIndicator.WaitActivationFromEvent);  //  hide on finishing movement way of figure
//        cardContext.DataObserver.ActivateCardObserver(cardContext.EventData);                                                ////////                                                                                           //movementWayService.OnSettingTarget.AddListener(dataObserver.DeactivateCardObserver);     //  re-sign on every move, otherwise the figure that was supposed to be
//                                                                                                             //  passed to the figure distributor wouldnt be updated, ----> on out of border instead
//                                                                                                             //  transition
//    }
//    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {}
//}

public class FigureDistributingState : CardFuncState {
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies){
        Debug.Log("FIGURE DISTRIBUTING STATE");
        dependencies.FigureDistributor.SwitchCardToFigure(cardContext.CurrentCard);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {}
}

public class FigureMovementState : CardFuncState {
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies) {
        GameService.GetService<FigureMovementService>().AddFigure(cardContext.CurrentFigure);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {}
}

