using Representation;
using UnityEngine;
////////////////////////////////////////////////////////
//  Managing creatiion of _objects and corresponding shit
////////////////////////////////////////////////////////

public class SceneBootrstrapper : MonoBehaviour
{
    //  Scriptable _objects
    [SerializeField]
    private CardSize CardSize;
    [SerializeField]
    private CellsMatrixData CellsMatrix;
    [SerializeField]
    private HandScale HandSize;
    //  Prefabs
    [SerializeField]
    private GameObject CardPrefab;
    [SerializeField]
    private GameObject CardReversePrefab;
    [SerializeField]
    private GameObject FigurePrefab;
    [SerializeField]
    private GameObject PositionIndicatorOfCardPrefab;
    [SerializeField]
    private Shader StandartShader;
    [SerializeField]
    private GameObject PlayerStatsParent;
    [SerializeField]
    private GameObject EnemyStatsParent;
    [SerializeField]
    private GameObject EnemysHand;
    [SerializeField]
    private GameObject PlayersHand;

    [SerializeField]
    private RectTransform EnemysHandRect;
    [SerializeField]
    private RectTransform PlayersHandRect;

    private void Start()
    {
        Debug.Log("Scene bootsrapper was started");

        //  hands
        var trailwayCanvas = GameObject.Find(CellsMatrix.ParentCanvas).transform;
        _playersHand = PlayersHand.GetComponent<Hand>();
        _enemysHand = EnemysHand.GetComponent<Hand>();
        
        //  

        //  scriptable obects data
        if (CardPrefab == null || CardSize == null || CellsMatrix == null)
        {
            Debug.Log("not determinated fields in hand script");
            return;
        }
        var cardPrefabSize = CardPrefab.GetComponent<RectTransform>();
        CardSize.CardScale.x = cardPrefabSize.rect.width;   //  determine the size of cards sprite
        CardSize.CardScale.y = cardPrefabSize.rect.height;   //  determine the size of cards sprite


        //  trailway - game field, + representation
        GameObject.Find("Trailway").GetComponent<Trailway>().Initialize();

        //  Intialization of services and dependent objects
        GetComponent<ServicesBootstrapper>().Initialize();
        var movementWayService = GameService.GetService<MovementWayService>();
        //var positionIndicator = new PositionIndicator(CellsMatrix, PositionIndicatorOfCardPrefab, trailwayCanvas);
        //positionIndicator.Initialize();
        MovementWayService.GetInstance().GetComponent<MovementWay>().Initialize(CellsMatrix, new GameObject(), trailwayCanvas, StandartShader);
       
        //  visual representation
        var cellsRepresentation = GameObject.Find("TrailwaysRepresentation");
        var representation = cellsRepresentation.GetComponent<SquareCellRepresentation>();
        representation.SetCellsTracker();
        if (cellsRepresentation)
        {
            representation.SetRepresentationCollection(CellsMatrix.Height * CellsMatrix.Width);
            for (int y = 0; y < CellsMatrix.Height; ++y)
                for (int i = 0; i < CellsMatrix.Width; ++i)
                    representation.PlaceCellOnPoint(CellsMatrix.TrailwayCentersOfCells[y][i]);
        }

        //  preparing shit
        IFabric cardFabric = new CardFabric(CardPrefab, PlayersHandRect);
        IFabric enemysCardFabric = new CardFabric(CardReversePrefab, EnemysHandRect);
        IFabric figureFabric = new FigureFabric(FigurePrefab, GameObject.Find(CellsMatrix.ParentCanvas).GetComponent<RectTransform>());
        //  create players
        var playerController = new PlayerController(); 
        var enemyController = new PlayerController();
        playerController.OnUpdateStats.AddListener(PlayerStatsParent.GetComponent<PlayerStatsView>().OnUpdateStats);
        enemyController.OnUpdateStats.AddListener(EnemyStatsParent.GetComponent<PlayerStatsView>().OnUpdateStats);
        PlayerStatsParent.GetComponent<PlayerStatsView>().OnUpdateStats(playerController.Stats);
        EnemyStatsParent.GetComponent<PlayerStatsView>().OnUpdateStats(enemyController.Stats);

        //  for hands
        //HandScale hand = PlayersHand.GetComponent<HandScale>();
        _dealer = PlayersHand.GetComponent<HandDistributor>();
        _dealer.Initialize();    //  inherited from image in nested canvas
        _enemyDealer = EnemysHand.GetComponent<HandDistributor>();
        _enemyDealer.Initialize();    //  inherited from image in nested canvas
        //  hands
        _cardPool = new ObjectPool<Card>(HandSize.MaximalDeckSize);
        _enemysCardPool = new ObjectPool<Card>(HandSize.MaximalDeckSize);

        // for figures

        //  figures
        _figurePool = new ObjectPool<Figure>(HandSize.MaximalDeckSize * 2);
        _figurePlacer = new FigureDistributor(_figurePool, _cardPool, CellsMatrix);

        //  commands
        BaseAttackSetter allyAttacker = new AllyAttackSetter(enemyController);
        BaseAttackSetter enemyAttacker = new EnemyAttackSetter(playerController);
        
        //  pools creation
        var currentCard = _cardPool.GetPool();
        var currentEnemyCard = _enemysCardPool.GetPool();
        
        _playersHand.Cards = _cardPool.GetPool();
        _enemysHand.Cards = _enemysCardPool.GetPool();
		////////////////////////////////////////////////////////
		//  SETTING UP CARDS
		//  - fabric call
		//  - event logic
		//  -
		//  -
		//GameStatesSwitcher stageSwitcher = new GameStatesSwitcher(tempCellsTracker, GameService.GetService<CardMovementService>(), GameService.GetService<MovementWayService>(), _figurePlacer, GameService.GetService<FigureMovementService>(), positionIndicator);
		CardFuncController cardRelatedStatesController = new CardFuncController(GameService.GetService<ICellsTracker>(), GameService.GetService<CardMovementService>(), GameService.GetService<MovementWayService>(), _figurePlacer, GameService.GetService<FigureMovementService>(), representation);
        for (int i = 0; i < HandSize.StartingHandSize; ++i){
			//  event stages bind
			//stageSwitcher.EnterCardDragDetectingState(currentCard[i].GetComponent<CardDragHandler>());
			// <--------------------------------------------------------------------------------------
			//  figures
			var tmp = _figurePool.GetPool()[i] = figureFabric.Create(new Vector3());    //  ERROR IS POSSIBLE!!!!!
            //tmp.name = $"Figure #{i}";
            var tmpCntrl = tmp.AddComponent<FigureAnimatorController>();
            tmp.GetComponent<Figure>().SetSide(allyAttacker);
            tmpCntrl.WorldTargetPosition = EnemysHandRect.transform.position;
            tmpCntrl.SetTargetPosition(EnemysHandRect.localPosition);
            //Debug.Log($"TARGET HAND POS = {tmpCntrl.WorldTargetPosition}");
        }
        for (int i = 0; i < _enemysCardPool.Size; ++i){
            currentEnemyCard[i] = enemysCardFabric.Create(new Vector3());
            
        }
        ////////
        //  events independent from current card

        //Timer
        GameService.Register<TimerMock>(new TimerMock()).Initialize();
        GameService.GetService<TimerMock>().OnEverySecond.AddListener(GameObject.Find("Timer").GetComponentInChildren<TimerRepresentation>().OnUpdateNumber);
        GameService.GetService<TimerMock>().OnTurnEnd.AddListener(GameObject.Find("EndTurnMessage").GetComponentInChildren<EndMessage>().ChangeMessage);
        GameService.GetService<TimerMock>().OnTurnEnd.AddListener(GameService.GetService<FigureMovementService>().PerformOnTurnEnd);
        GameService.GetService<TimerMock>().OnTurnEnd.AddListener(playerController.PerformOnTurnEnd);
        GameService.GetService<TimerMock>().OnTurnEnd.AddListener(enemyController.PerformOnTurnEnd);
        GameService.GetService<TimerMock>().OnTurnEnd.AddListener(GameService.GetService<FigureAttackingService>().PerformOnTurnEnd);
        ////////
        
        //  
        Debug.Log("Cards distributing");
        _dealer.DistributeCards(_playersHand.Cards);
        _enemyDealer.DistributeCards(_enemysHand.Cards);
        //_dealer.DistributeCards(_cardPool.GetPool());
        //_enemyDealer.DistributeCards(_enemysCardPool.GetPool());
        
        //  atp all figures are disabled, cards[] of StartingHandSize.StartingHandSize laying in the hand and the other are disabled too
        
        Debug.Log("Scene was completed");

    }

    //  contiguous things
    private FigureDistributor _figurePlacer;
    private Hand _playersHand;
    private Hand _enemysHand;
    private HandDistributor _dealer;
    private HandDistributor _enemyDealer;
    private ObjectPool<Card> _cardPool;
    private ObjectPool<Card> _enemysCardPool;
    private ObjectPool<Figure> _figurePool;
}