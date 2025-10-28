using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

class FigureMovementService: IService, ITurnBaseLogic{
    public FigureMovementService(CellsMatrixData cellsMatrix) { 
        _cellsMatrix = cellsMatrix;
    }
    public void Initialize(){
        _spaceInPointsArray = _cellsMatrix.Width * _cellsMatrix.Height;
        _points = new List<Vector3[]>();
        //new Vector3[];
    }
    public void AddFigure(GameObject figure) {
        _figures.Add(figure);
    }
    public void AddMovementWay(Vector3[] points) {
        _points.Add(new Vector3[_spaceInPointsArray]);
        Array.Copy(points, _points[_points.Count - 1], points.Length);

    }
    public void PerformOnTurnEnd() {
        for (int i = 0; i < _figures.Count; ++i) {
            if (_points[i][0] != null) { 
                _figures[i].transform.position = _points[i][0];
                for (int j = 0; j < _points.Count - 1; ++j)
                    _points[i][j] = _points[i][j + 1];
                _points[i][_points.Count - 1] = new Vector3();
            }
            else{
                _figures.RemoveAt(i);
                _points.RemoveAt(i);
            }
        }
           
    }

    //
    private List<GameObject> _figures = new List<GameObject>();   //  without fixed limit of figures on field
    private List<Vector3[]> _points;
    private int _limitOnField = 0;   //  limit of figures on field
    private CellsMatrixData _cellsMatrix;
    private int _turnCounter;
    private int _spaceInPointsArray;
    //
    //
    //


}