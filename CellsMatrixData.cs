using UnityEngine;
[CreateAssetMenu(fileName = "CellsMatrixData", menuName = "ScriptableObject/CellsMatrixData")]
public class CellsMatrixData : ScriptableObject
{
    //
    [SerializeField]
    private int _width; //  amount horizontal
    [SerializeField]    //  amount vertical
    private int _height;
    [SerializeField]
    private float _spacingHorizontal;
    [SerializeField]
    private float _spacingVertical;
    [SerializeField]
    private float _offsetTop;
    [SerializeField]
    private float _offsetLeft;
    [SerializeField]
    private float _zPointOfDraggedItems = 40.0f; 

    //
    public Vector3 CellSize { get { return _cellSize; } set { _cellSize = value; } }
    public Vector3[] FieldRectSize { get { return _fieldRectSize; } set { _fieldRectSize = value; } }
    public Vector3[][] TrailwayCentersOfCells { get { return _trailwayCentersOfCells; } set { _trailwayCentersOfCells = value; } }
    public string ParentCanvas { get { return _parentCanvas; } }
    public string CellsRepresentation { get { return _cellsRepresentation; } }
    public int Width { get { return _width; } set { _width = value; } }
    public int Height { get { return _height; } set { _height = value; } }
    public float OffsetTop { get { return _offsetTop; } }
    public float OffsetLeft { get { return _offsetLeft; } }
    public float SpacingHorizontal { get { return _spacingHorizontal; } }
    public float SpacingVertical { get { return _spacingVertical; } }
    public float ZPointOnDrag { get { return _zPointOfDraggedItems; } }
    //

    private Vector3 _cellSize; // x - width, y - height, z - deep
    private Vector3[][] _trailwayCentersOfCells;    // world space
    private Vector3[] _fieldRectSize;
    private const string _parentCanvas = "Trailground";
    private const string _cellsRepresentation = "Trailway";
}
