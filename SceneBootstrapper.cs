using UnityEngine;
using Representation;
using UnityEngine.EventSystems;

public class SceneBootrstrapper : MonoBehaviour
{
    //  Scriptable objects
    [SerializeField]
    private HandScale HandSize;
    [SerializeField]
    private CardSize CardSize;  
    [SerializeField]
    private CellsMatrixData CellsMatrix;
    //  Prefabs
    [SerializeField]
    private GameObject CardPrefab;
    [SerializeField]
    private GameObject PositionIndicatorOfCardPrefab;

    private void Start()
    {
        Debug.Log("Scene bootsrapper was started");
        
        //  hands
        var CardsHand = GameObject.Find("CardsHand");
        if (CardsHand == null)
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

        //  Intialization of services
        GetComponent<ServicesBootstrapper>().Initialize();
        var positionIndicator = new PositionIndicator(CellsMatrix, PositionIndicatorOfCardPrefab, GameObject.Find(CellsMatrix.ParentCanvas).transform);
        positionIndicator.Initialize();

        //  visual representation
        //var cellsRepresentation = GameObject.Find("TrailwaysRepresentation");
        //if (cellsRepresentation)
        //{
        //    var representation = cellsRepresentation.GetComponent<SquareCellRepresentation>();
        //    representation.SetRepresentationCollection(CellsMatrix.Height * CellsMatrix.Width);
        //    for (int y = 0; y < CellsMatrix.Height; ++y)
        //        for (int i = 0; i < CellsMatrix.Width; ++i)
        //            representation.PlaceCellOnPoint(CellsMatrix.TrailwayCentersOfCells[y][i]);
        //}


        //  create players

        //  preparing shit for hands
        //HandScale hand = CardsHand.GetComponent<HandScale>();
        HandScale hand = this.HandSize;
        HandDistributor dealer = CardsHand.GetComponent<HandDistributor>();
        dealer.Initialize();    //  inherited from image in nested canvas
        
        //  hands
        CardPool cardPool = new CardPool();
        cardPool.SetPool(hand.HandSize * 3);

        //  cards
        IFabric cardFabric = new CardFabric(CardPrefab);
        for (int i = 0; i < cardPool.Size; ++i)
        {
            cardPool.GetPool()[i] = cardFabric.Create(new Vector3());
            if (cardPool.GetPool()[i] == null)
                Debug.LogError($"Card #{i} is null");
            //  events which depending from actual cards
            cardPool.GetPool()[i].GetComponent<CardDragHandler>().OnCardDragEnd.AddListener(positionIndicator.WaitActivationFromEvent);
        }

        for (int i = hand.HandSize; i < cardPool.Size; ++i)
        {
            cardPool.GetPool()[i].SetActive(false);
        }

        Debug.Log("Cards distributing");
        dealer.DistributeCards(cardPool.GetPool());
        Debug.Log("Scene was completed");

    }
}