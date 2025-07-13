using Representation;
using UnityEngine;
using UnityEngine.XR;

public class SceneBootrstrapper: MonoBehaviour 
{
    [SerializeField]
    private CardSize CardSize;
    [SerializeField]
    private GameObject CardPrefab;
    [SerializeField]
    private CellsMatrixData CellsMatrix;

    private void Start()
    {
        Debug.Log("Scene bootsrapper was started");

        //  

        //  scriptable obects data
        if (CardPrefab == null || CardSize == null || CellsMatrix == null) {
            Debug.Log("not determinated fields in hand script");
            return;
        }
        var cardPrefabSize = CardPrefab.GetComponent<RectTransform>();
        CardSize.CardScale.x = cardPrefabSize.rect.width;   //  determine the size of cards sprite
        CardSize.CardScale.y = cardPrefabSize.rect.height;   //  determine the size of cards sprite
        

        //  trailway - game field, + representation
        GameObject.Find("Trailway").GetComponent<Trailway>().Initialize();
        //  Intialization of services
        var cellsRepresentation = GameObject.Find("TrailwaysRepresentation");
        if (cellsRepresentation){
            var representation = cellsRepresentation.GetComponent<SquareCellRepresentation>();
            representation.SetRepresentationCollection(CellsMatrix.Height * CellsMatrix.Width);
            for (int y = 0; y < CellsMatrix.Height; ++y)
                for (int i = 0; i < CellsMatrix.Width; ++i)
                    representation.PlaceCellOnPoint(CellsMatrix.TrailwayCentersOfCells[y][i]);
        }

        //  services
        GetComponent<ServicesBootstrapper>().Initialize();

        //  preparing shit
        var hand = GameObject.Find("CardsHand").GetComponent<Hand>();
        hand.CardFabric = new CardFabric(CardPrefab, hand.transform);   //  inherited from image in nested canvas
        hand.Initialize();
        
        
    }
}