using UnityEngine;
//////////////////////////
// Dependent from CellsMatrixData, GameService, each service, should be attached to bootstrapper
//////////////////////////
public class ServicesBootstrapper : MonoBehaviour
{
    //  ...
    [SerializeField]
    private CellsMatrixData _cellsMatrix;
    public void Initialize()
    {
        Debug.Log("Service bootstrapper proceeds");
        //
        GameService.Initialize();
        //  Trailway - model of the gamefield, should be initialized first
        ((CellsTrackerService)GameService.Register<ICellsTracker>(new CellsTrackerService(_cellsMatrix))).Initialize();   //  position on the field in the end of the drag
        ((CardMovementService)GameService.Register<CardMovementService>(new CardMovementService(_cellsMatrix))).Initialize();
        ((MovementWayService)GameService.Register<MovementWayService>(new MovementWayService())).Initialize();
        ((FigureMovementService)GameService.Register<FigureMovementService>(new FigureMovementService(_cellsMatrix))).Initialize();
        //
    }

    //  ���������� ��� ������������ on card drag begin �� card movement service

    public void OnStartCardMovement() { 
    
    }
    public void OnEndCardMovement() { 

    }
    //  ������� �������� ����������� ����� �� startcardmoovement, ������� ���� ��������� �� �������

}
