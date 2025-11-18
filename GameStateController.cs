//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.EventSystems;
////////////////////////////////////////////////////////////////////////////
/////States:
////1 - обработчик взаимодействия с картой(CardDragHandler) - не влияет на cells tracker
////2 - сервис движения карты(CardMovementService)
////3 - сервис прокладывания пути(MovementWayService)
////4 - передатчик данных карты(CardDataObserver)
////5 - установка фигуры(FigureDistributor)
////6 - движение фигуры(FigureMovementService)
////side states:
////23 - position indicator
////
////dependencies:
////cells tracker: 
////on out of border - 2, 3, 4
////on cell change - 3, 23
////on return in border - 2, 4,
////calculate current cell - 2, 3 
////////////////////////////////////////////////////////////////////////////

//public class GameStatesSwitcher {
//    public GameStatesSwitcher(ICellsTracker tracker, CardMovementService cardMove, MovementWayService way, FigureDistributor figureDistributor, FigureMovementService figureMove, PositionIndicator indicator)
//    {
//        _cellsTracker = tracker;
//        _cardMovementService = cardMove;   //  1
//        _movementWayService = way;
//        _figureDistributor = figureDistributor;
//        _figureMovementService = figureMove;
//        _positionIndicator = indicator;
//        if (_cellsTracker == null || _cardMovementService == null || _movementWayService == null || _figureDistributor == null || _figureMovementService == null || _positionIndicator == null)
//            Debug.LogError("Invalid conctruction in game state switcher");
       
//        //  dependencies which should have been set in their initializers 
//        _cellsTracker.GetOnOutOfBorder().AddListener(_cardMovementService.ReturnInHandOnEndDrag);
//        _cellsTracker.GetOnReturnInBorder().AddListener(_cardMovementService.PlaceOnFieldOnEndDrag);
//    }
//    public void TurnOnCardRelatedMechanics() { 
        
//    }
//    public void EnterCardDragDetectingState(CardDragHandler dragHandler) {
//        dragHandler.OnCardDragStart.AddListener(EnterCardMovementState);    //  should be permanent subscripted
//    } 
//    public void EnterCardMovementState(PointerEventData eventData) {
//        //_currentDragHandler.OnCardDragStart.AddListener(_cardMovementService.OnBeginDrag);
//        _currentDataObserver = _cardMovementService.GetCurrentCard().GetComponent<CardDataObserver>();
//        _currentDragHandler = _cardMovementService.GetCurrentCard().GetComponent<CardDragHandler>();
//        _cardMovementService.OnBeginDrag(eventData);
//        _currentDragHandler.OnCardDragEnd.AddListener(_cardMovementService.OnEndDrag);
//        _currentDragHandler.OnCardDrag.AddListener(_cardMovementService.OnDrag);
//        _currentDragHandler.OnCardDragEnd.AddListener(_positionIndicator.WaitActivationFromEvent);               //  hide indicator for the next cells tracker call 

//        _cellsTracker.GetOnReturnInBorder().AddListener(StartPosIndication);
//        //  transition
//        _currentDragHandler.OnCardDragEnd.AddListener(EnterSettingMovementWayState);                       //  this will cause the path to be created extra times every time, when card is let out of field
//    }
        
//    public void EnterSettingMovementWayState(PointerEventData eventData) {
//        _movementWayService.StartMakingWay(eventData);
//        _cellsTracker.GetOnOutOfBorder().AddListener(_movementWayService.CancelMakingWay);
//        _movementWayService.OnInterruptingMovementSetting.AddListener(_cardMovementService.ReturnLastCardInTheHand);
//        //  transition
//        _movementWayService.OnSettingTarget.AddListener(EnterCardDataTransferState);                     //  -> figure movement service
//    }
//    public void EnterCardDataTransferState(PointerEventData eventData) {
//        //_movementWayService.OnSettingTarget.AddListener(_positionIndicator.WaitActivationFromEvent);  //  hide on finishing movement way of figure
//        _currentDataObserver.ActivateCardObserver(eventData);                                                ////////                                                                                           //movementWayService.OnSettingTarget.AddListener(dataObserver.DeactivateCardObserver);     //  re-sign on every move, otherwise the figure that was supposed to be
//                                                                                                        //  passed to the figure distributor wouldnt be updated, ----> on out of border instead
//        //  transition
//        _currentDataObserver.OnPushingData.AddListener(EnterDistributingFigureState);          //  listening figure data observer from distributor -> figure movement service
//    }
//    public void EnterDistributingFigureState(GameObject currentCard) {
//        _figureDistributor.SwitchCardToFigure(currentCard);
//        //  transition
//        _figureDistributor.OnEndSwitching.AddListener(EnterFigureMovementState);    //  -> figure movement, figure array
//    }
//    public void EnterFigureMovementState(GameObject currentFigure) {
//        GameService.GetService<FigureMovementService>().AddFigure(currentFigure);
//    }
//    public void StartPosIndication() {
//        _cellsTracker.GetOnReturnInBorder().RemoveListener(StartPosIndication);
//        _positionIndicator.ActivateTracking();
//        _cellsTracker.GetOnCellChange().AddListener(_positionIndicator.ChangeCurrentPosition);
//        _cellsTracker.GetOnOutOfBorder().AddListener(_positionIndicator.Hide);
//        _cellsTracker.GetOnOutOfBorder().AddListener(_positionIndicator.WaitForCellsTracker);
//    }
//    public void EndPosIndication(){
//        _positionIndicator.DeactivateTracking();
//        _cellsTracker.GetOnCellChange().RemoveListener(_positionIndicator.ChangeCurrentPosition);
//        _cellsTracker.GetOnOutOfBorder().RemoveListener(_positionIndicator.Hide);
//        _cellsTracker.GetOnOutOfBorder().RemoveListener(_positionIndicator.WaitForCellsTracker);
//        _cellsTracker.GetOnCellChange().RemoveListener(_positionIndicator.Reset);
//    }
//    //
//    private enum _states { 
//        DragDetection,
//        SettingMovementWay,
//        DataTransfrer,
//        DistributingFigures,
//        FigureMovement
//    }
//    private CardDragHandler                 _currentDragHandler;    //  0
//    private CardDataObserver                _currentDataObserver;   //  3
    
//    private readonly ICellsTracker          _cellsTracker;
//    private readonly CardMovementService    _cardMovementService;   //  1
//    private readonly MovementWayService     _movementWayService;    //  2
//    private readonly FigureDistributor      _figureDistributor;     //  4
//    private readonly FigureMovementService  _figureMovementService; //  5
//    private readonly PositionIndicator      _positionIndicator;
//}