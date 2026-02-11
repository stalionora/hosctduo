using System;
using UnityEngine;
using UnityEngine.Events;
////////////////////////////////////////////////////////////////////////
// dependent from cellsMatrixData
// provides current cell of trailground, which places under the cursor 
////////////////////////////////////////////////////////////////////////
//  ВЕРОЯТНО, НЕ ДОЛЖЕН БЫТЬ СЕРВИСОМ
public class CellsTrackerService : IService, ICellsTracker{
    //  INTERFACE
    public UnityEvent<Vector3> OnCellChange = new();
    public UnityEvent OnOutOfBorder = new();
    public UnityEvent OnReturnInBorder = new();

    //  constructor, which is called from servicebootstrapper
    public CellsTrackerService(CellsMatrixData cellsMatrixData){
        _cellsMatrixData = cellsMatrixData;
    }
    public void Initialize(){
        if (_cellsMatrixData.TrailwayCentersOfCells == null){
            Debug.LogError("Trailway GameObject not found!");
            return;
        }
        if (_cellsMatrixData.TrailwayCentersOfCells[0][0].x < 0 || _cellsMatrixData.TrailwayCentersOfCells[0][0].y < 0){
            _offsetOfTheLowestPoint.x = Mathf.Abs(_cellsMatrixData.TrailwayCentersOfCells[0][0].x);
            _offsetOfTheLowestPoint.y = Mathf.Abs(_cellsMatrixData.TrailwayCentersOfCells[0][0].y);
            //OnOutOfBorder.AddListener(() = > );
        }
        OnOutOfBorder.AddListener(OutOfBorder);
    }

    // returns world coordinates of cell
    public Vector3 GetCurrentCellCoordinates(){
        return _currentMatrixPosition;
    }

    public int GetCurrentCellNumber() {
        return _number;
    }

    public UnityEvent<Vector3> GetOnCellChange(){
        return OnCellChange;
    }

    public UnityEvent GetOnOutOfBorder(){
        return OnOutOfBorder;
    }

    public UnityEvent GetOnReturnInBorder(){
        return OnReturnInBorder;
    }

    ///////////////////////////////
    //  MAJOR FUNCTIONALITY [ WRONG ]
    //  - не учитываются отступы
    //  - не учитывается возможность отсутствия определения offsetOfTheLowestPoint
    public void CalcuateCurrentCell(Vector3 pointOnCanvas){
        //  INVARIANT: pointOnCanvas is within boundaries of trailground
        //  POINT ON TRAILWAY
        //  TO FLOAT
        //  TO OPTIMIZE
        //Debug.Log("cell calculation");
        if ((pointOnCanvas - _lastPointOnCanvas).sqrMagnitude > 0.001f){
            if ((pointOnCanvas.x >= _cellsMatrixData.TrailwayCentersOfCells[0][0].x - _cellsMatrixData.CellSize.x / 2) && (pointOnCanvas.x < _cellsMatrixData.TrailwayCentersOfCells[_cellsMatrixData.Height - 1][_cellsMatrixData.Width - 1].x + _cellsMatrixData.CellSize.x / 2)){
                if ((pointOnCanvas.y >= _cellsMatrixData.TrailwayCentersOfCells[0][0].y - _cellsMatrixData.CellSize.y / 2) && (pointOnCanvas.y < _cellsMatrixData.TrailwayCentersOfCells[_cellsMatrixData.Height - 1][_cellsMatrixData.Width - 1].y + _cellsMatrixData.CellSize.y / 2)){
                    OnReturnInBorder.Invoke();
                    _lastNumberOfCell = _number;
                    _lastPointOnCanvas = pointOnCanvas;
                    _number = CalculateCellByPoint(pointOnCanvas);
                    CalculateCurrentCoordinates();
                    if (_number != _lastNumberOfCell)
                        OnCellChange.Invoke(_currentMatrixPosition);
                    //Debug.Log("on cell change");
                    return;
                }
            }
            //if (_isOutOfBorder == false){
            OnOutOfBorder.Invoke();
                
            //}
        }
    }

    public int CalculateCellByPoint(Vector3 pointOnCanvas) {
        return Mathf.FloorToInt((pointOnCanvas.x - _cellsMatrixData.TrailwayCentersOfCells[0][0].x + _cellsMatrixData.CellSize.x / 2) / _cellsMatrixData.CellSize.x) + Mathf.FloorToInt((pointOnCanvas.y - _cellsMatrixData.TrailwayCentersOfCells[0][0].y + _cellsMatrixData.CellSize.y / 2) / _cellsMatrixData.CellSize.y) * _cellsMatrixData.Width;  //  calculating 
    }

    public Vector3 GetPointByCellNumber(int number) {   //  direct from array of points in cells matrix data
        if (number >= 0 && number < _cellsMatrixData.Width * _cellsMatrixData.Height)
            return _cellsMatrixData.TrailwayCentersOfCells[number / _cellsMatrixData.Width][(number % _cellsMatrixData.Width)];
        else return new Vector3(float.NaN, float.NaN, float.NaN);
    }
    
    ////////////////////////////////////////////////////////////////////////
    //  realization
    private void CalculateCurrentCoordinates() {
        //if (_currentMatrixPosition != _number)

        //Debug.Log($"{_number / (_cellsMatrixData.Width)} and  {_number % _cellsMatrixData.Width}");
        if (_number >=0 && _number < _cellsMatrixData.Width * _cellsMatrixData.Height)
            _currentMatrixPosition = _cellsMatrixData.TrailwayCentersOfCells[_number / _cellsMatrixData.Width][(_number % _cellsMatrixData.Width)];
        //Debug.Log(_currentMatrixPosition);
    }
    private void ReturnInBorder() {
        OnOutOfBorder.AddListener(OutOfBorder);
        OnReturnInBorder.RemoveListener(ReturnInBorder);

    }
    private void OutOfBorder() { 
        _number = int.MinValue;
        OnReturnInBorder.AddListener(ReturnInBorder);
        OnOutOfBorder.RemoveListener(OutOfBorder);
    
    }

    //fields
    private int _number = int.MinValue;
    private int _lastNumberOfCell = int.MaxValue;

    //fields which only presenting trailways values
    private Vector3 _currentMatrixPosition;
    private Vector3 _lastPointOnCanvas;
    private Vector3 _offsetOfTheLowestPoint = new();

    //private Vector3 _positionOfCursor;
    private CellsMatrixData _cellsMatrixData;
}