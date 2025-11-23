using Representation;
using TMPro;
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
    private GameObject FigurePrefab;
    [SerializeField]
    private GameObject PositionIndicatorOfCardPrefab;
    [SerializeField]
    private Material StandartShader;

    private void Start()
    {
        Debug.Log("Scene bootsrapper was started");

        //  hands
        var PlayersHand = GameObject.Find("PlayersHand");
        var trailwayCanvas = GameObject.Find(CellsMatrix.ParentCanvas).transform;
        _playersHand = PlayersHand.GetComponent<Hand>();
        if (_playersHand == null)
        {
            Debug.Log("not determinated fields in hand script");
            return;
        }
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

        //  Intialization of services and depenent objects

        GetComponent<ServicesBootstrapper>().Initialize();
        var movementWayService = GameService.GetService<MovementWayService>();
        var positionIndicator = new PositionIndicator(CellsMatrix, PositionIndicatorOfCardPrefab, trailwayCanvas);
        positionIndicator.Initialize();
        MovementWayService.GetInstance().GetComponent<MovementWay>().Initialize(CellsMatrix, new GameObject(), trailwayCanvas, StandartShader);
        //  visual representation
        var cellsRepresentation = GameObject.Find("TrailwaysRepresentation");
        if (cellsRepresentation)
        {
            var representation = cellsRepresentation.GetComponent<SquareCellRepresentation>();
            representation.SetRepresentationCollection(CellsMatrix.Height * CellsMatrix.Width);
            for (int y = 0; y < CellsMatrix.Height; ++y)
                for (int i = 0; i < CellsMatrix.Width; ++i)
                    representation.PlaceCellOnPoint(CellsMatrix.TrailwayCentersOfCells[y][i]);
        }

        //  preparing shit
        IFabric cardFabric = new CardFabric(CardPrefab);
        IFabric figureFabric = new FigureFabric(FigurePrefab, GameObject.Find(CellsMatrix.ParentCanvas).GetComponent<RectTransform>());
        //  create players

        //  for hands
        //HandScale hand = PlayersHand.GetComponent<HandScale>();
        _dealer = PlayersHand.GetComponent<HandDistributor>();
        _dealer.Initialize();    //  inherited from image in nested canvas

        //  hands
        _cardPool = new ObjectPool<Card>(HandSize.HandSize * 3);

        // for figures

        //  figures
        _figurePool = new ObjectPool<Figure>(HandSize.HandSize * 3);
        _figurePlacer = new FigureDistributor(_figurePool, _cardPool, CellsMatrix);
        
        //  pools creation
        var currentCard = _cardPool.GetPool();
        
        _playersHand.Cards = _cardPool.GetPool();
        ////////////////////////////////////////////////////////
        //  SETTING UP CARDS
        //  - fabric call
        //  - event logic
        //  -
        //  -
        //GameStatesSwitcher stageSwitcher = new GameStatesSwitcher(tempCellsTracker, GameService.GetService<CardMovementService>(), GameService.GetService<MovementWayService>(), _figurePlacer, GameService.GetService<FigureMovementService>(), positionIndicator);
        CardFuncController cardRelatedStatesController = new CardFuncController(GameService.GetService<ICellsTracker>(), GameService.GetService<CardMovementService>(), GameService.GetService<MovementWayService>(), _figurePlacer, GameService.GetService<FigureMovementService>(), positionIndicator);
        for (int i = 0; i < _cardPool.Size; ++i){
            currentCard[i] = cardFabric.Create(new Vector3());
            if (currentCard == null)
                Debug.LogError($"Card #{i} is null");

            /////////
            //  event stages bind
            //stageSwitcher.EnterCardDragDetectingState(currentCard[i].GetComponent<CardDragHandler>());
            
            cardRelatedStatesController.SetSwitchingRuleOfExecutionForCurrentCard(currentCard[i].GetComponent<CardDragHandler>());
            currentCard[i].name = $"Card #{i}";
            // <--------------------------------------------------------------------------------------
            //  figures
            _figurePool.GetPool()[i] = figureFabric.Create(new Vector3());
            _figurePool.GetPool()[i].name = $"Figure #{i}";
        }
        ////////
        //  events independent from current card

        //Timer
        GameService.Register<TimerMock>(new TimerMock()).Initialize();
        GameService.GetService<TimerMock>().OnEverySecond.AddListener(GameObject.Find("Timer").GetComponentInChildren<TimerRepresentation>().OnUpdateNumber);
        GameService.GetService<TimerMock>().OnTurnEnd.AddListener(GameObject.Find("EndTurnMessage").GetComponentInChildren<EndMessage>().ChangeMessage);
        GameService.GetService<TimerMock>().OnTurnEnd.AddListener(GameService.GetService<FigureMovementService>().PerformOnTurnEnd);
        ////////
        
        //  
        Debug.Log("Cards distributing");
        _dealer.DistributeCards(_cardPool.GetPool());
        
        //  atp all figures are disabled, cards[] of HandSize.HandSize laying in the hand and the other are disabled too
        
        Debug.Log("Scene was completed");

    }

    //  contiguous things
    private FigureDistributor _figurePlacer;
    private Hand _playersHand;
    private HandDistributor _dealer;
    private ObjectPool<Card> _cardPool;
    private ObjectPool<Figure> _figurePool;
}