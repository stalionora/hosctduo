using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public abstract class CardRelatedState{
    public abstract void Enter(ref CardContext cc, ref DependencyContext dc);
    public abstract void Exit(ref CardContext cc, ref DependencyContext dc);
    
}

public class DragDetectionState : CardRelatedState {
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies) {
        cardContext.DragHandler.OnCardDragStart.AddListener(dependencies.CardMovementService.OnBeginDrag);    //  should be permanent subscripted
        cardContext.DragHandler.OnCardDragEnd.AddListener(dependencies.CardMovementService.OnEndDrag);
        cardContext.DragHandler.OnCardDrag.AddListener(dependencies.CardMovementService.OnDrag);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {    //  can be optimized
        cardContext.DragHandler.OnCardDragStart.RemoveListener(dependencies.CardMovementService.OnBeginDrag);    //  should be permanent subscripted
        cardContext.DragHandler.OnCardDragEnd.RemoveListener(dependencies.CardMovementService.OnEndDrag);
        cardContext.DragHandler.OnCardDrag.RemoveListener(dependencies.CardMovementService.OnDrag);
    }
}
public class CardMovementState : CardRelatedState {
    public override void Enter(ref CardContext cardContext,ref DependencyContext dependencies){
        //_currentDragHandler.OnCardDragStart.AddListener(_cardMovementService.OnBeginDrag);
        dependencies.CardMovementService.OnBeginDrag(cardContext.EventData);
        
                    

        //dependencies.CellsTracker.GetOnReturnInBorder().AddListener(dependencies.StartPosIndication); //  Turning on PositionIndicator
        //  transition


    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {
        dependencies.PositionIndicator.WaitActivationFromEvent();   //  hide indicator for the next cells tracker call 
    }
}
public class SettingMovementWayState : CardRelatedState {
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies){
        dependencies.MovementWayService.StartMakingWay(cardContext.EventData);
        dependencies.CellsTracker.GetOnOutOfBorder().AddListener(dependencies.MovementWayService.CancelMakingWay);
        dependencies.MovementWayService.OnInterruptingMovementSetting.AddListener(dependencies.CardMovementService.ReturnLastCardInTheHand);
        //  transition
        //_movementWayService.OnSettingTarget.AddListener(EnterCardDataTransferState);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {}
}
public class DataTransferState: CardRelatedState { 
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies){
        //_movementWayService.OnSettingTarget.AddListener(_positionIndicator.WaitActivationFromEvent);  //  hide on finishing movement way of figure
        cardContext.DataObserver.ActivateCardObserver(cardContext.EventData);                                                ////////                                                                                           //movementWayService.OnSettingTarget.AddListener(dataObserver.DeactivateCardObserver);     //  re-sign on every move, otherwise the figure that was supposed to be
                                                                                                             //  passed to the figure distributor wouldnt be updated, ----> on out of border instead
                                                                                                             //  transition
        //_currentDataObserver.OnPushingData.AddListener(EnterDistributingFigureState);          //  listening figure data observer from distributor -> figure movement service
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {}
}
public class FigureDistributingState : CardRelatedState {
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies){
        dependencies.FigureDistributor.SwitchCardToFigure(cardContext.CurrentCard);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {}
}
public class FigureMovementState : CardRelatedState {
    public override void Enter(ref CardContext cardContext, ref DependencyContext dependencies) {
        GameService.GetService<FigureMovementService>().AddFigure(cardContext.CurrentFigure);
    }
    public override void Exit(ref CardContext cardContext, ref DependencyContext dependencies) {}
}

