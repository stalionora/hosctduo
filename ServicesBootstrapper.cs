using UnityEngine;
using Representation;
//////////////////////////
// Dependent from CellsMatrixData, GameService, each service
//////////////////////////
public class ServicesBootstrapper : MonoBehaviour
{
    
    public void Initialize()
    {
        Debug.Log("Service bootstrapper proceeds");
        //
        GameService.Initialize();
        //  Trailway - model of the gamefield, should be initialized first

        //
    }

    //  вызывается при срабатывании on card drag begin из card movement service
    public void StartCardMovement(Vector3 coordinatesOfCard, CellsMatrixData matrix) {
            ((CellsTrackerService)GameService.Register<ICellsTracker>(new CellsTrackerService(matrix))).Initialize();
            //GameObject.Find("CardDragHandler").GetComponent<CardMovementService>().Initialize();   // толsько для одной карты
            GameObject.Find("PositionIndicator").GetComponent<PositionIndicator>().Initialize(coordinatesOfCard); // ИНИЦИАЛИЗИРОВАТЬ ПОСЛЕ СОЗДАНИЯ КАРТЫ ИЗ ФАБРИКИ 
    }

    //  функция подписки создаваемой карты на startcardmoovement, которая сама подписана на фабрику
    //

    
}
