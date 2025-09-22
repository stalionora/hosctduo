using System;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;
//////////////////////////////////////////////
//  depending from 
//////////////////////////////////////////////

public class MovementWay: ReusableObject {         
    //  добавить внешний ивент, который будет отрубать этот объект, который должен бы быть синглтоном, и триггерить пуш данных 
    //  вынести в отдельный класс трекинг мыши 
    public void Initialize(CellsMatrixData cellsMatrixData, GameObject prefab, Transform parentCanvas, Material shader) {
        _cellsTracker = GameService.GetService<ICellsTracker>();
        _movementWay = new GameObject();
        _movementWay.transform.SetParent(parentCanvas);
        _movementWay.AddComponent<MeshRenderer>();
        _movementWay.AddComponent<MeshFilter>();
        _mesh = new Mesh();
        _movementWay.GetComponent<MeshFilter>().mesh = _mesh;
        var renderer = _movementWay.GetComponent<MeshRenderer>();
        renderer.material = new Material(shader);
        renderer.material.color = Color.red;
    } 

    //  movement related
    public void SetTriangles(){   //  final
    }
    public void AddPoint(Vector3 newPoint) {
        //if ()
        _mid = (newPoint + _lastPoint) / 2;  // var
        _difference = new Vector3(Mathf.Abs(newPoint.x - _lastPoint.x), Mathf.Abs(newPoint.y - _lastPoint.y));
        _horizontalMultiplier = _difference.normalized.y * _width / 2f; 
        _verticalMultiplier = _difference.normalized.x * _width / 2f;
        _cloclwiseVertices.Add(new Vector3((_mid - _difference).x + _horizontalMultiplier, (_mid - _difference).y + _verticalMultiplier));
        _cloclwiseVertices.Add(new Vector3((_mid + _difference).x + _horizontalMultiplier, (_mid + _difference).y + _verticalMultiplier));
        _cloclwiseVertices.Add(new Vector3((_mid - _difference).x - _horizontalMultiplier, (_mid - _difference).y - _verticalMultiplier));
        _cloclwiseVertices.Add(new Vector3((_mid + _difference).x - _horizontalMultiplier, (_mid + _difference).y - _verticalMultiplier));
        _points.Add(newPoint);

        for (int i = 0; i < 6; ++i) //  from  
            _trianglesOrder.Add(6 * _points.Count + i);
        //  rendering
        _mesh.triangles = _trianglesOrder.ToArray();
        _mesh.vertices = _cloclwiseVertices.ToArray();
        _mesh.RecalculateNormals();
        _lastPoint = newPoint;
    }
    public void StartMakingWay(GameObject draggedCard) {
        _cellsTracker.GetOnCellChange().AddListener(AddPoint);
        _points.Add(draggedCard.transform.position);
        for (int i = 0; i < 6; ++i) //  from  
            _trianglesOrder.Add(i);

    }
    //  
    public void Hide() {
        _mesh.RecalculateBounds();
        _mesh.RecalculateBounds();
        _mesh.triangles = _trianglesOrder.ToArray();
        _mesh.vertices = _cloclwiseVertices.ToArray();
        _movementWay.SetActive(false);
        _lastPoint = new Vector3();
    }

    //////////////////////////////////////////////
    //  form
    public void ShapingTheCornerBridges() { }
    //////////////////////////////////////////////

    private ICellsTracker _cellsTracker;
    private GameObject _movementWay;
    private Mesh _mesh;
    private Vector3 _lastPoint = new();
    private Vector3 _mid = new();
    private Vector3 _difference = new();
    private List<Vector3> _cloclwiseVertices = new List<Vector3>();
    private List<int> _trianglesOrder = new List<int>();
    private List<Vector3> _points = new List<Vector3>();

    //private float _bridge = 0f;
    private float _width = 20f; //  width of line
    private float _horizontalMultiplier = 0f;
    private float _verticalMultiplier = 0f;
    private int _rounding = 0; 
    private enum TypeOfBridge { None = 0, Arrow = 1, PointMaker = 2};
    private TypeOfBridge BridgesType = TypeOfBridge.None;
    
    //////////////////////////////////////////////
}