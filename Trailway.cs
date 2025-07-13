// 
using System;
using UnityEngine;
/// ////////////////////////////////////////////////////
//It is points, through which the figure can be dragged
//should be child of background
//Dependent from CellsMatrixData 
////////////////////////////////////////////////////////
public class Trailway : MonoBehaviour
{
    [SerializeField]
    private CellsMatrixData _cellsMatrixData;
   
    //public RectTransform Rectangle { get { return _rectangle; } }
    /////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake() // receiving 
    {
        _cellsMatrixData.FieldRectSize = new Vector3[4];
        GameObject.Find(_cellsMatrixData.ParentCanvas).GetComponent<RectTransform>().GetWorldCorners(_cellsMatrixData.FieldRectSize);
    }
    public void Initialize()
    {
        _cellsMatrixData.TrailwayCentersOfCells = new Vector3[_cellsMatrixData.Height][];
        for (int i = 0; i < _cellsMatrixData.Height; ++i)
            _cellsMatrixData.TrailwayCentersOfCells[i] = new Vector3[_cellsMatrixData.Width];
        //  ui ����������, getworldcorners ���� �� ������� ������� �� ���� - ���� 0, 3, 1, 2, ���� 3, 0, 2, 1
        //  ���������� ������������ ���� � ������ ���� ��������
        //  1/2 - first position x and y, 3/4 - spacing, 4 - debug func
        //  rg[0]-o-s/2-c-s-c-s-c-s-c-s-c-s/2-o-rg[3]   (c - card, o - offset, s - spacing)
        //  distance should be divided through amount of spacings: width - 1
        _cellsMatrixData.CellSize = new Vector3(((_cellsMatrixData.FieldRectSize[3].x - _cellsMatrixData.FieldRectSize[0].x - _cellsMatrixData.OffsetLeft * 2 - _cellsMatrixData.SpacingHorizontal * (_cellsMatrixData.Width - 1)) / (_cellsMatrixData.Width)), ((_cellsMatrixData.FieldRectSize[1].y - _cellsMatrixData.FieldRectSize[0].y - _cellsMatrixData.OffsetTop * 2 - _cellsMatrixData.SpacingVertical * (_cellsMatrixData.Height - 1)) / (_cellsMatrixData.Height)), 0);
        DistributePlace(_cellsMatrixData.FieldRectSize[0].x + _cellsMatrixData.OffsetLeft + _cellsMatrixData.CellSize.x / 2, _cellsMatrixData.FieldRectSize[0].y + _cellsMatrixData.OffsetTop + _cellsMatrixData.CellSize.y / 2, _cellsMatrixData.CellSize.x, _cellsMatrixData.CellSize.y, null);
        //  debug
        //for (int y = 0; y < _cellsMatrixData.Height; ++y)
        //    for (int i = 0; i < _cellsMatrixData.Width; ++i)
        //        Debug.Log($"Cells matrix data Trailway[{y}][{i}]= {_cellsMatrixData.TrailwayCentersOfCells[y][i]}");
    }


    //
    private void DistributePlace(float startPointX, float startPointY, float widthDistance, float heightDistance, Action<Vector3> DebugRepresentation = null)    //uses distance between points
    {
        //foreach (var corner in _rectGroundParent) 
        //    Debug.Log($"{corner}");
        for (int y = 0; y < _cellsMatrixData.Height; ++y) 
        {
            for (int i = 0; i < _cellsMatrixData.Width; ++i)
            {
                // distance is > 0?
                _cellsMatrixData.TrailwayCentersOfCells[y][i] = new Vector3 (widthDistance * (i) + startPointX + _cellsMatrixData.SpacingHorizontal * i, heightDistance * (y) + startPointY + _cellsMatrixData.SpacingVertical * y, GetComponentInParent<Canvas>().transform.position.z);
                //Debug.Log($"TrailwayCentersOfCells[{y}][{i}] = {_cellsMatrixData.TrailwayCentersOfCells[y][i]}");
            }
            //Debug.Log($"������ ������� = {_cellsMatrixData.TrailwayCentersOfCells[y].Length}");
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////
    
    //private RectTransform _rectangle;
    //private GameObject[] _imagesConcept = new GameObject[_amount * (_amount + 1)];
};