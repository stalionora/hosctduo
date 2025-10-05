using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
//////////////////////////////////////////////
//  depending from: IcellsTracker, MouseTracker
//////////////////////////////////////////////


public class MovementWay : MonoBehaviour{
    //  добавить внешний ивент, который будет отрубать этот объект, который должен бы быть синглтоном, и триггерить пуш данных 
    //  вынести в отдельный класс трекинг мыши 
    public void Awake()
    {
        enabled = false;
        _mainCamera = Camera.allCameras[0];
    }
    public void Initialize(CellsMatrixData cellsMatrixData, GameObject prefab, Transform parentCanvas, Material shader)
    {

        _cellsTracker = GameService.GetService<ICellsTracker>();
        _mouseTracker = new MouseTracker(GameObject.Find(cellsMatrixData.ParentCanvas).transform.position);
        this.transform.gameObject.AddComponent<MeshRenderer>();
        this.transform.gameObject.AddComponent<MeshFilter>();
        this.transform.gameObject.name = "MovementWay";
        _mesh = new Mesh();
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = new Material(shader);
        renderer.material.color = Color.gray;
        enabled = false;
        _halfWidth = cellsMatrixData.CellSize.y / 7f;
        _mid.z = cellsMatrixData.FieldRectSize[0].z;
        this.transform.position = _mid;
    }
    //private void Update()
    //{
    //    _cellsTracker.CalcuateCurrentCell(_mouseTracker.TrackMouse(Input.mousePosition));
    //}
    //  movement related
    public void SetTriangles()
    {   //  final
    }
    public void AddPoint(Vector3 newPoint)
    {
        Debug.Log($"point from cells tracker is equal to: {newPoint}");
        //newPoint.x /= transform.localScale.x;
        //newPoint.y /= transform.localScale.y;
        Debug.LogFormat($"_rectScaler = {_rectScaler}");
        _points.Add(newPoint);
        //if (_points.Count < 2)
        //{
        //    _lastPoint = newPoint;
        //    return;
        //}
        _startingIndex = _cloclwiseVertices.Count;
        _mid = (newPoint + _lastPoint) / 2f;  // var
        //Assert.AreEqual(_mid.z, _zPoint);
        _majorLineMultiplier = new Vector3(Mathf.Abs(newPoint.x - _lastPoint.x), Mathf.Abs(newPoint.y - _lastPoint.y));
        //_majorLineMultiplier = new Vector3(newPoint.x - _lastPoint.x, newPoint.y - _lastPoint.y);
        _horizontalMultiplier = _majorLineMultiplier.normalized.y * _halfWidth;
        _verticalMultiplier = _majorLineMultiplier.normalized.x * _halfWidth;
        _majorLineMultiplier.x /= 2f;
        _majorLineMultiplier.y /= 2f;

        //_cloclwiseVertices.Add(new Vector3(_mid.x + _majorLineMultiplier.x - _horizontalMultiplier, _mid.y - _majorLineMultiplier.y - _verticalMultiplier, newPoint.z - Vector3.one.z));
        //_cloclwiseVertices.Add(new Vector3(_mid.x - _majorLineMultiplier.x - _horizontalMultiplier, _mid.y + _majorLineMultiplier.y - _verticalMultiplier, newPoint.z - Vector3.one.z));
        //_cloclwiseVertices.Add(new Vector3(_mid.x - _majorLineMultiplier.x + _horizontalMultiplier, _mid.y + _majorLineMultiplier.y + _verticalMultiplier, newPoint.z - Vector3.one.z));
        //_cloclwiseVertices.Add(new Vector3(_mid.x + _majorLineMultiplier.x + _horizontalMultiplier, _mid.y - _majorLineMultiplier.y + _verticalMultiplier, newPoint.z - Vector3.one.z));
        _cloclwiseVertices.Add(new Vector3(_mid.x + _majorLineMultiplier.x - _horizontalMultiplier, _mid.y - _majorLineMultiplier.y - _verticalMultiplier, 0 - Vector3.one.z));
        _cloclwiseVertices.Add(new Vector3(_mid.x - _majorLineMultiplier.x - _horizontalMultiplier, _mid.y + _majorLineMultiplier.y - _verticalMultiplier, 0 - Vector3.one.z));
        _cloclwiseVertices.Add(new Vector3(_mid.x - _majorLineMultiplier.x + _horizontalMultiplier, _mid.y + _majorLineMultiplier.y + _verticalMultiplier, 0 - Vector3.one.z));
        _cloclwiseVertices.Add(new Vector3(_mid.x + _majorLineMultiplier.x + _horizontalMultiplier, _mid.y - _majorLineMultiplier.y + _verticalMultiplier, 0 - Vector3.one.z));
        _trianglesOrder.Add(0 + _startingIndex);
        _trianglesOrder.Add(1 + _startingIndex);
        _trianglesOrder.Add(2 + _startingIndex);
        _trianglesOrder.Add(3 + _startingIndex);
        _trianglesOrder.Add(0 + _startingIndex);
        _trianglesOrder.Add(2 + _startingIndex);


        //  rendering
        _mesh.vertices = _cloclwiseVertices.ToArray();
        _mesh.triangles = _trianglesOrder.ToArray();
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = _mesh;
        _lastPoint = newPoint;
        Debug.Log($"points count = {_points.Count}");
    }

    //  managing
    public void StartMakingWay(ref GameObject draggedCard)
    {
        //_rectScaler = draggedCard.GetComponentInParent<RectTransform>().localScale;
        //enabled = true;
        _cellsTracker.GetOnCellChange().AddListener(AddPoint);
        _points.Add(_cellsTracker.GetCurrentCellCoordinates());
        _lastPoint = _points[0];
    }

    //  
    public void Reset()
    {
        _mesh.triangles = null;
        _mesh.vertices = null;
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        _cloclwiseVertices.Clear();
        _points.Clear();
        _trianglesOrder.Clear();
        _lastPoint = new Vector3();
        _cellsTracker.GetOnCellChange().RemoveListener(AddPoint);
        enabled = false;
        _horizontalMultiplier = 0f;
        _verticalMultiplier = 0f;

    }

    //////////////////////////////////////////////
    //  form
    public void ShapingTheCornerBridges() { }
    //////////////////////////////////////////////

    private Camera _mainCamera;
    private Mesh _mesh;
    private Vector3 _lastPoint = new();
    private Vector3 _mid = new();
    private Vector3 _majorLineMultiplier = new();
    private Vector3 _rectScaler = new();
    private List<Vector3> _cloclwiseVertices = new List<Vector3>();
    private List<Vector3> _points = new List<Vector3>();
    private List<int> _trianglesOrder = new List<int>();
    private ICellsTracker _cellsTracker;
    private MouseTracker _mouseTracker;
    //private float _bridge = 0f;
    private float _halfWidth = 5.0f; //  width of line
    private float _horizontalMultiplier = 0f;
    private float _verticalMultiplier = 0f;
    private int _rounding = 0;
    private int _startingIndex = 0;
    private enum TypeOfBridge { None = 0, Arrow = 1, PointMaker = 2 };
    private TypeOfBridge BridgesType = TypeOfBridge.None;

    //////////////////////////////////////////////
}