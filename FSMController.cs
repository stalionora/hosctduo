using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
public class FSMController {
    // setting permanent dependency context
    public FSMController(ICellsTracker cellsTracker, CardMovementService cardMovementService, MovementWayService movementWayService, 
    FigureDistributor figureDistributor, FigureMovementService figureMovementService, PositionIndicator positionIndicator) { 
        // context
        _dependencyContext = new DependencyContext();
        _cardContext = new CardContext();
        _dependencyContext.CellsTracker = cellsTracker;
        _dependencyContext.CardMovementService = cardMovementService;
        _dependencyContext.MovementWayService = movementWayService;
        _dependencyContext.FigureDistributor = figureDistributor;
        _dependencyContext.FigureMovementService = figureMovementService;
        _dependencyContext.PositionIndicator = positionIndicator;
        // states
        _dragDetectionState = new DragDetectionState();
        _cardMovementState = new CardMovementState();
        _settingMovementWayState = new SettingMovementWayState();
        _dataTransferState = new DataTransferState();
        _figureDistributingState = new FigureDistributingState();
        _figureMovementState = new FigureMovementState();
    }
    public void SetSwitchOrder(CardDragHandler dragHandler, CardDataObserver dataObserver){ //  for current card
        _cardContext.DragHandler = dragHandler;
        _cardContext.DataObserver = dataObserver;
        SwitchToDragDetectionState();
        _cardContext.DragHandler.OnCardDragStart.AddListener(SwitchToCardMovementState);
        _cardContext.DragHandler.OnCardDragEnd.AddListener(SwitchToSettingMovementWayState);
        _dependencyContext.MovementWayService.OnSettingTarget.AddListener(SwitchToDataTransferState);
        _cardContext.DataObserver.OnPushingData.AddListener(SwitchToFigureDistributingState);
        _dependencyContext.FigureDistributor.OnEndSwitching.AddListener(SwitchToFigureMovementState);
    }
    public void SwitchToDragDetectionState() {  //  base state  BASE
        _cardContext.DragHandler.OnDetectingCurrentDraggedCard.AddListener(SetCardInContext);
        EnterWithState(_dragDetectionState);
    }
    public void SwitchToCardMovementState(PointerEventData eventData) {
        _dependencyContext.CardMovementService.SetCurrentCard(ref _cardContext.CurrentCard);
        _cardRelatedState = _cardMovementState;
        _cardContext.EventData = eventData;
        _cardContext.DragHandler = _cardContext.CurrentCard.GetComponent<CardDragHandler>();
        _cardContext.DataObserver = _cardContext.CurrentCard.GetComponent<CardDataObserver>();
        EnterWithState(_cardMovementState);
    }
    public void SwitchToSettingMovementWayState(PointerEventData eventData) { 
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.EventData = eventData;
        EnterWithState(_settingMovementWayState);
    }
    public void SwitchToDataTransferState(PointerEventData eventData) { 
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.EventData = eventData;
        EnterWithState(_dataTransferState);
    }   
    public void SwitchToFigureDistributingState(GameObject currentFigure) {
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.CurrentFigure = currentFigure;
        EnterWithState(_figureDistributingState);
    }
    public void SwitchToFigureMovementState(GameObject currentFigure) { 
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.CurrentFigure = currentFigure;
        EnterWithState(_figureMovementState);
    }

    private void EnterWithState(CardRelatedState newState) { 
        _cardRelatedState = newState;
        newState.Enter(ref _cardContext, ref _dependencyContext);
    }

    private void SetCardInContext(GameObject currentCard) { 
        _cardContext.CurrentCard = currentCard;
    }
    //  contexts
    private DependencyContext _dependencyContext;
    private CardContext _cardContext;
    private CardRelatedState _cardRelatedState;

    //
    private readonly DragDetectionState _dragDetectionState;
    private readonly CardMovementState _cardMovementState;
    private readonly SettingMovementWayState _settingMovementWayState;
    private readonly DataTransferState _dataTransferState;
    private readonly FigureDistributingState _figureDistributingState;
    private readonly FigureMovementState _figureMovementState;
    
    private const int _amountOfStates = 6;

}

public class DependencyContext {
    public ICellsTracker CellsTracker;
    public CardMovementService CardMovementService;       //  1
    public MovementWayService MovementWayService;         //  2
    public FigureDistributor FigureDistributor;           //  4
    public FigureMovementService FigureMovementService;   //  5
    public PositionIndicator PositionIndicator;
}

public class CardContext{
    public CardDragHandler DragHandler;
    public CardDataObserver DataObserver;
    public PointerEventData EventData;
    public GameObject CurrentCard;
    public GameObject CurrentFigure;
}