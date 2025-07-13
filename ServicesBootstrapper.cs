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

    //  ���������� ��� ������������ on card drag begin �� card movement service
    public void StartCardMovement(Vector3 coordinatesOfCard, CellsMatrixData matrix) {
            ((CellsTrackerService)GameService.Register<ICellsTracker>(new CellsTrackerService(matrix))).Initialize();
            //GameObject.Find("CardDragHandler").GetComponent<CardMovementService>().Initialize();   // ���s��� ��� ����� �����
            GameObject.Find("PositionIndicator").GetComponent<PositionIndicator>().Initialize(coordinatesOfCard); // ���������������� ����� �������� ����� �� ������� 
    }

    //  ������� �������� ����������� ����� �� startcardmoovement, ������� ���� ��������� �� �������
    //

    
}
