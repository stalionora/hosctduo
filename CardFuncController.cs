using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Representation;
using UnityEngine.InputSystem.LowLevel;
public class CardFuncController {
    // setting permanent dependency context
    public CardFuncController(ICellsTracker cellsTracker, CardMovementService cardMovementService, MovementWayService movementWayService,
    FigureDistributor figureDistributor, FigureMovementService figureMovementService, SquareCellRepresentation visualShit) {
        // context
        _dependencyContext = new DependencyContext();
        _cardContext = new CardContext();
        _dependencyContext.CellsTrackerService = cellsTracker;
        _dependencyContext.CardMovementService = cardMovementService;
        _dependencyContext.MovementWayService = movementWayService;
        _dependencyContext.FigureDistributor = figureDistributor;
        _dependencyContext.FigureMovementService = figureMovementService;
        _dependencyContext.VisualShit = visualShit;
        //_dependencyContext.PositionIndicator = positionIndicator;
        // states
        _dragDetectionState = new DragDetectionState();
        _cardMovementState = new CardMovementState();
        _settingMovementWayState = new SettingMovementWayState();
        _figureDistributingState = new FigureDistributingState();
        _figureMovementState = new FigureMovementState();
        //  rules
        SetPermanentRuleOfExecution();
    }

    public void SetSwitchingRuleOfExecutionForCurrentCard(CardDragHandler dragHandler) { //  for current card
        _currentCardState = _dragDetectionState;    //  entrance state
        _cardContext.DragHandler = dragHandler;
        //  drag detection
        SwitchToDragDetectionState();
        _cardContext.DragHandler.OnDetectingCurrentDraggedCard.AddListener(SetCardInContext); //    all drag handlers schould send their card on drag begin
        _cardContext.DragHandler.OnCardDragStart.AddListener(SwitchToCardMovementState);
        _cardContext.DragHandler.OnCardDragEnd.AddListener(SwitchToSettingMovementWayState);                 //    setting movement way
                                                                                              //    will called after on end drag, even when movement was cancelled
    }

    private void SetPermanentRuleOfExecution() {
        _dependencyContext.MovementWayService.OnSettingTarget.AddListener(SwitchToFigureDistributingState);    //  figure distributing
        _dependencyContext.FigureDistributor.OnEndSwitching.AddListener(SwitchToFigureMovementState);
        //_dependencyContext.CardMovementService.OnInterruptingMovement.AddListener(InterruptingSwitchingToBaseState);    //  when cancelling
        _dependencyContext.MovementWayService.OnInterruptingMovementSetting.AddListener(InterruptingSwitchingToBaseState);    //  when cancelling
        _dependencyContext.FigureMovementService.OnEndOfWay.AddListener(GameService.GetService<FigureAttackingService>().SetAttacker);
    }

    private void DeleteRulesOfExecution() {
        _cardContext.DragHandler.OnDetectingCurrentDraggedCard.RemoveListener(SetCardInContext);  //  exit of drag detecting state
        _cardContext.DragHandler.OnCardDragStart.RemoveListener(SwitchToCardMovementState);
        _cardContext.DragHandler.OnCardDragEnd.RemoveListener(SwitchToSettingMovementWayState);    //  will called after on end drag, even when movement was cancelled
    }

    private void DeletePermanentRulesOfExecution() { 
        _dependencyContext.MovementWayService.OnSettingTarget.RemoveListener(SwitchToFigureDistributingState);
        _dependencyContext.FigureDistributor.OnEndSwitching.RemoveListener(SwitchToFigureMovementState);
        //_dependencyContext.CardMovementService.OnInterruptingMovement.RemoveListener(InterruptingSwitchingToBaseState);    //  when cancelling
        _dependencyContext.MovementWayService.OnInterruptingMovementSetting.RemoveListener(InterruptingSwitchingToBaseState);    //  when cancelling
    }

    //////////////////////////////////////////////////////////////////////////
    //  switchers
    private void SwitchToDragDetectionState(PointerEventData useless = null) {  //  in this state fsm entrances only when set switching rule is called
        _currentCardState.Exit(ref _cardContext, ref _dependencyContext);
        EnterWithState(_dragDetectionState);
    }
    
    private void SwitchToCardMovementState(PointerEventData eventData) {
        _currentCardState.Exit(ref _cardContext, ref _dependencyContext);
        _dependencyContext.CardMovementService.SetCurrentCard(ref _cardContext.CurrentCard);    //  sends value of current card to movement service
        _cardContext.DragHandler = _cardContext.CurrentCard.GetComponent<CardDragHandler>();
        _cardContext.EventData = eventData;
        EnterWithState(_cardMovementState);
    }
    
    private void SwitchToSettingMovementWayState(PointerEventData eventData) {   //   
        _currentCardState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.EventData = eventData;
        EnterWithState(_settingMovementWayState);
    }
    
    private void SwitchToFigureDistributingState(PointerEventData useless) {
        _currentCardState.Exit(ref _cardContext, ref _dependencyContext);
        EnterWithState(_figureDistributingState);
    }
    
    private void SwitchToFigureMovementState(GameObject currentFigure) {
        _currentCardState.Exit(ref _cardContext, ref _dependencyContext);
        _cardContext.CurrentFigure = currentFigure;
        EnterWithState(_figureMovementState);
        DeleteRulesOfExecution();
    }

    //////////////////////////////////////////////////////////////////////////
    //  secondary 
    private void RequestSwitching(PointerEventData data) {
        if (_isSwitching == true){
            Debug.Log("REQUEST WAS REFUSED");
            return;
        }
        else if (_queuedSwitcher != null){
            Debug.Log("REQUEST WAS ACCEPTED");
            _isSwitching = true;
            _queuedSwitcher.Invoke(data);
            _isSwitching = false;
            _queuedSwitcher = null;
        }
    }

    private void InterruptingSwitchingToBaseState() {
        _queuedSwitcher = null;
        SwitchToDragDetectionState();
    }

    public void PutInterrupterInQueue() {
        _queuedSwitcher = SwitchToDragDetectionState;
    }

    private void PutSwitcherInQueue(Action<PointerEventData> switcher) {
        _queuedSwitcher = switcher ;
    } 

    private void EnterWithState(CardFuncState newState) { 
        _currentCardState = newState;
        newState.Enter(ref _cardContext, ref _dependencyContext);
    }

    //private void ExitFromCurrentStateTo(CardFuncState state) {
    //    if (_currentCardState == state){
    //        //state.Exit(ref _cardContext, ref _dependencyContext);
    //        Debug.Log("Correct exit from state!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1");
    //    }
    //    else ExitFromCurrentStateTo(_currentCardState);  
    //}
    
    private void SetCardInContext(GameObject currentCard){
        _cardContext.CurrentCard = currentCard;
    }
    
    //////////////////////////////////////////////////////////////////////
    //private void SetFSMSwitchingEntrance(Action<PointerEventData> switcher, UnityEvent<PointerEventData> switchingCase)
    //{
    //    _switcher = switcher;
    //    _switchingCase = switchingCase;
    //    _switchingCase.AddListener(OnStartStateTriggered);
    //}
    //private void OnStartStateTriggered(PointerEventData data)
    //{
    //    _switcher.Invoke(data);
    //}
    //private void RemoveFSMSwitchingEntrance()
    //{
    //    _switchingCase.RemoveListener(OnStartStateTriggered);
    //}
    /// ///////////////////////////////////////////////////////////////////
    //entrance case and state
    //private Action<PointerEventData> _switcher;
    //private UnityEvent<PointerEventData> _switchingCase;
    
    //  states
    private CardFuncState _currentCardState;
    private bool _isSwitching = false;
    private Action<PointerEventData> _queuedSwitcher;

    //  context
    private DependencyContext _dependencyContext;
    private CardContext _cardContext;

    //  switcher states
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
    public SquareCellRepresentation VisualShit;       //  6
    //public PositionIndicator PositionIndicator;
}

public class CardContext{
    public CardDragHandler DragHandler;
    public PointerEventData EventData;
    public GameObject CurrentCard;
    public GameObject CurrentFigure;
}