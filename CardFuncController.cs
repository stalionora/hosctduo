using System;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class CardFuncController {
    // setting permanent dependency context
    public CardFuncController(ICellsTracker cellsTracker, CardMovementService cardMovementService, MovementWayService movementWayService, 
    FigureDistributor figureDistributor, FigureMovementService figureMovementService, PositionIndicator positionIndicator) { 
        // context
        _dependencyContext = new DependencyContext();
        _cardContext = new CardContext();
        _dependencyContext.CellsTrackerService = cellsTracker;
        _dependencyContext.CardMovementService = cardMovementService;
        _dependencyContext.MovementWayService = movementWayService;
        _dependencyContext.FigureDistributor = figureDistributor;
        _dependencyContext.FigureMovementService = figureMovementService;
        _dependencyContext.PositionIndicator = positionIndicator;
        // states
        _dragDetectionState = new DragDetectionState();
        _cardMovementState = new CardMovementState();
        _settingMovementWayState = new SettingMovementWayState();
        _figureDistributingState = new FigureDistributingState();
        _figureMovementState = new FigureMovementState();
        //  rules
        SetPermanentRuleOfExecution();
        _cardRelatedState = _dragDetectionState;
    }
    
    public void SetSwitchingRuleOfExecutionForCurrentCard(CardDragHandler dragHandler){ //  for current card
        _cardContext.DragHandler = dragHandler;
        //  normal execution
        SwitchToDragDetectionState();
        _cardContext.DragHandler.OnCardDragStart.AddListener(SwitchToCardMovementState);
        _cardContext.DragHandler.OnCardDragEnd.AddListener(SwitchToSettingMovementWayState);
        //  +
        //
        //  when cancelling
        _dependencyContext.CellsTrackerService.GetOnOutOfBorder().AddListener(SwitchToDragDetectionState);
    }
    
    public void SetPermanentRuleOfExecution(){
        
        _dependencyContext.MovementWayService.OnSettingTarget.AddListener(SwitchToFigureDistributingState);
        _dependencyContext.FigureDistributor.OnEndSwitching.AddListener(SwitchToFigureMovementState);
    }

    public void SwitchToDragDetectionState() {  //  secondary state
        //_cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.DragHandler.OnDetectingCurrentDraggedCard.AddListener(SetCardInContext);
        //SetFSMSwitchingEntrance(SwitchToCardMovementState, _cardContext.DragHandler.OnCardDragStart);
        EnterWithState(_dragDetectionState);
    }
    
    public void SwitchToCardMovementState(PointerEventData eventData) {
        _dependencyContext.CardMovementService.SetCurrentCard(ref _cardContext.CurrentCard);
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.DragHandler = _cardContext.CurrentCard.GetComponent<CardDragHandler>();
        _cardRelatedState = _cardMovementState;
        _cardContext.EventData = eventData;
        EnterWithState(_cardMovementState);
        //RemoveFSMSwitchingEntrance();
        _cardContext.DragHandler.OnDetectingCurrentDraggedCard.RemoveListener(SetCardInContext);
    }
    
    public void SwitchToSettingMovementWayState(PointerEventData eventData) { 
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.EventData = eventData;
        EnterWithState(_settingMovementWayState);
    }
    
    public void SwitchToFigureDistributingState(PointerEventData useless) {
        _cardContext.DragHandler.OnDetectingCurrentDraggedCard.RemoveListener(SetCardInContext);
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        EnterWithState(_figureDistributingState);
    }
    
    public void SwitchToFigureMovementState(GameObject currentFigure) { 
        _cardRelatedState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.CurrentFigure = currentFigure;
        EnterWithState(_figureMovementState);
    }

    private void EnterWithState(CardFuncState newState) { 
        _cardRelatedState = newState;
        newState.Enter(ref _cardContext, ref _dependencyContext);
    }
    //
    
    private void SetCardInContext(GameObject currentCard) { 
        _cardContext.CurrentCard = currentCard;
    }

    //private void SetFSMSwitchingEntrance(Action<PointerEventData> switcher, UnityEvent<PointerEventData> switchingCase)
    //{
    //    _switcher = switcher;
    //    _switchingCase = switchingCase;
    //    _switchingCase.AddListener(OnStartStateTriggered);
    //}
    //private void OnStartStateTriggered(PointerEventData data)
    //{
    //    _switcher?.Invoke(data);
    //}
    //private void RemoveFSMSwitchingEntrance()
    //{
    //    _switchingCase.RemoveListener(OnStartStateTriggered);
    //}
    //entrance case and state
    private Action<PointerEventData> _switcher;
    private UnityEvent<PointerEventData> _switchingCase;
    //
    private CardFuncState _cardRelatedState;
    //  context
    private DependencyContext _dependencyContext;
    private CardContext _cardContext;

    private readonly DragDetectionState _dragDetectionState;
    private readonly CardMovementState _cardMovementState;
    private readonly SettingMovementWayState _settingMovementWayState;
    private readonly FigureDistributingState _figureDistributingState;
    private readonly FigureMovementState _figureMovementState;
}

public class DependencyContext {
    public ICellsTracker CellsTrackerService;
    public CardMovementService CardMovementService;       //  1
    public MovementWayService MovementWayService;         //  2
    public FigureDistributor FigureDistributor;           //  4
    public FigureMovementService FigureMovementService;   //  5
    public PositionIndicator PositionIndicator;
}

public class CardContext{
    public CardDragHandler DragHandler;
    public PointerEventData EventData;
    public GameObject CurrentCard;
    public GameObject CurrentFigure;
}