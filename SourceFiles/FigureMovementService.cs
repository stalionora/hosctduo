using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FigureMovementService : IService, ITurnBaseLogic {
    public UnityEvent<Figure> OnEndOfWay = new();
    public FigureMovementService(CellsMatrixData cellsMatrix) {
        _cellsMatrix = cellsMatrix;
    }
    public void Initialize() {
        _spaceInPointsArray = _cellsMatrix.Width * _cellsMatrix.Height;
        _points = new List<Vector3[]>();
        //new Vector3[];
    }
    public void AddFigure(GameObject figure) {
        Debug.Log("Figure added");
        _figures.Add(figure);
    }
    public void AddMovementWay(Vector3[] points) {
        Debug.Log("Movement way added");
        _points.Add(new Vector3[_spaceInPointsArray]);
        var currentPointsIndex = _points.Count() - 1;
        Array.Copy(points, 0L, _points[currentPointsIndex], 0, points.Length);
        for (int i = points.Length; i < _spaceInPointsArray; ++i)  //  filling array with invalid numbers
            _points[currentPointsIndex][i] = _outOfPointsRange;
        _points[currentPointsIndex][0] = _outOfPointsRange; //  removins first element, which have been used in making way representation
        for (int i = 0; i < _spaceInPointsArray - 2; ++i)   //  SHIFT
            _points[currentPointsIndex][i] = _points[_points.Count() - 1][i + 1];

    }
public void PerformOnTurnEnd() {
        Debug.Log("End of turn");
        Debug.Log($"Figures count = {_figures.Count}");
        Debug.Log($"points count = {_points.Count}");
        for (int i = 0; i < _figures.Count; ++i)
        {
            if (_points[i][0] != _outOfPointsRange) {
                _figures[i].transform.position = _points[i][0];
                //_figures[i].transform.Translate(_points[i][0] - _figures[i].transform.position);
                ShiftSegmentLeft(i);
            }
            else{
                OnEndOfWay.Invoke(_figures[i].GetComponent<Figure>());
                DeleteFigureFromList(i);
                --i;    //  to compensate remove of object
            }
        }
    }

    private void ShiftSegmentLeft(int pointsIter){
        for (int j = 0; j < _spaceInPointsArray - 2; ++j)

            _points[pointsIter][j] = _points[pointsIter][j + 1];

        _points[pointsIter][_spaceInPointsArray - 1] = _outOfPointsRange;
    }
    private void DeleteFigureFromList(int pos) { 
        _figures.RemoveAt(pos);
        _points.RemoveAt(pos);
    }
    //
    private List<GameObject> _figures = new List<GameObject>();   //  without fixed limit of figures on field
    private List<Vector3[]> _points;
    private CellsMatrixData _cellsMatrix;
    private Vector3 _outOfPointsRange = new Vector3(float.MinValue, float.MinValue, float.MinValue);

    private int _spaceInPointsArray;
}