using UnityEngine;
using UnityEngine.Events;

public interface ICellsTracker
{
    int CalculateCellByPoint(Vector3 pointOnCanvas);
    int GetCurrentCellNumber();
    void CalcuateCurrentCell(Vector3 pointOnCanvas);
    Vector3 GetPointByCellNumber(int number);
    Vector3 GetCurrentCellCoordinates();
    UnityEvent<Vector3> GetOnCellChange();  //  returns Unity event
    UnityEvent GetOnOutOfBorder(); //  to deactivate position indicator
    UnityEvent GetOnReturnInBorder(); //  to deactivate position indicator
    
}