using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
////////////////////////////////////////////////////////////////////////
// dependent from cellsMatrixData
// provides current cell of trailground, which places under the cursor 
////////////////////////////////////////////////////////////////////////
//  ВЕРОЯТНО, НЕ ДОЛЖЕН БЫТЬ СЕРВИСОМ
public class CellsTrackerService : IService, ICellsTracker
{
    //  INTERFACE
    public UnityEvent<Vector3> OnCellChange = new();
    public UnityEvent OnOutOfBorder = new();

    //  constructor, which is called from servicebootstrapper
    public CellsTrackerService(CellsMatrixData cellsMatrixData)
    {
        _cellsMatrixData = cellsMatrixData;
    }
    public void Initialize()
    {
        if (_cellsMatrixData.TrailwayCentersOfCells == null)
        {
            Debug.LogError("Trailway GameObject not found!");
            return;
        }
        if (_cellsMatrixData.TrailwayCentersOfCells[0][0].x < 0 || _cellsMatrixData.TrailwayCentersOfCells[0][0].y < 0)
        {
            _offsetOfTheLowestPoint.x = Mathf.Abs(_cellsMatrixData.TrailwayCentersOfCells[0][0].x);
            _offsetOfTheLowestPoint.y = Mathf.Abs(_cellsMatrixData.TrailwayCentersOfCells[0][0].y);
            _offsetOfTheLowestPoint.z = 0;
        }
    }

    // returns world coordinates of cell
    public Vector3 GetCurrentCellCoordinates()
    {
        return _currentMatrixPosition;
    }
    public UnityEvent<Vector3> GetOnCellChange()
    {
        return OnCellChange;
    }
    public UnityEvent GetOnOutOfBorder()
    {
        return OnOutOfBorder;
    }
    ///////////////////////////////
    //  MAJOR FUNCTIONALITY [ WRONG ]
    //  - не учитываются отступы
    //  - не учитывается возможность отсутствия определения offsetOfTheLowestPoint
    public void CalcuateCurrentCell(Vector3 pointOnCanvas)
    {
        //  INVARIANT: pointOnCanvas is within boundaries of trailground
        //  POINT ON TRAILWAY
        //  TO FLOAT
        //  TO OPTIMIZE

        if ((pointOnCanvas - _lastPointOnCanvas).sqrMagnitude > 0.001f)
        {
            if ((pointOnCanvas.x >= _cellsMatrixData.TrailwayCentersOfCells[0][0].x - _cellsMatrixData.CellSize.x / 2) && (pointOnCanvas.x < _cellsMatrixData.TrailwayCentersOfCells[_cellsMatrixData.Height - 1][_cellsMatrixData.Width - 1].x + _cellsMatrixData.CellSize.x / 2))
            {
                if ((pointOnCanvas.y >= _cellsMatrixData.TrailwayCentersOfCells[0][0].y - _cellsMatrixData.CellSize.y / 2) && (pointOnCanvas.y < _cellsMatrixData.TrailwayCentersOfCells[_cellsMatrixData.Height - 1][_cellsMatrixData.Width - 1].y + _cellsMatrixData.CellSize.y / 2))
                {
                    var coordinateByY = pointOnCanvas.y - _cellsMatrixData.TrailwayCentersOfCells[0][0].y;
                    var coordinateByX = pointOnCanvas.x - _cellsMatrixData.TrailwayCentersOfCells[0][0].x;
                    _number = Mathf.FloorToInt((coordinateByX + _cellsMatrixData.CellSize.x / 2) / _cellsMatrixData.CellSize.x) + Mathf.FloorToInt((coordinateByY + _cellsMatrixData.CellSize.y / 2) / _cellsMatrixData.CellSize.y) * _cellsMatrixData.Width;  //  calculating 
                    _lastPointOnCanvas = pointOnCanvas;

                    CalculateCurrentCoordinates();
                    OnCellChange.Invoke(_currentMatrixPosition);
                    _isOutOfBorder = false;
                    return;
                }
            }
            //if (_isOutOfBorder == false){
                OnOutOfBorder.Invoke();
                //_isOutOfBorder = true;
            //}
        }
    }

    ////////////////////////////////////////////////////////////////////////
    //  realization
    private void CalculateCurrentCoordinates()  //
    {
        //if (_currentMatrixPosition != _number)

        //Debug.Log($"{_number / (_cellsMatrixData.Width)} and  {_number % _cellsMatrixData.Width}");
        if (_number >=0 &_number < _cellsMatrixData.Width * _cellsMatrixData.Height)
            _currentMatrixPosition = _cellsMatrixData.TrailwayCentersOfCells[_number / _cellsMatrixData.Width][(_number % _cellsMatrixData.Width)];
    }


    //fields
    private int _number = 0;
    private bool _isOutOfBorder = false;

    //fields which only presenting trailways values
    private Vector3 _currentMatrixPosition;
    private Vector3 _lastPointOnCanvas;
    private Vector3 _offsetOfTheLowestPoint = new();

    //private Vector3 _positionOfCursor;
    private CellsMatrixData _cellsMatrixData;
}