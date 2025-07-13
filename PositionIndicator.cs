//  subscribe to on drag in cardmovementservice
//  check newPosition separately from cellstracker and make backlight on that cell
//  Depending only on icellstracker and cellsatrixdata

using UnityEngine;

public class PositionIndicator: MonoBehaviour
{   
    //  interface
    [SerializeField]
    private CellsMatrixData CellsMatrixData;
    
    public void Initialize(Vector3 firstCellsCoordinates) 
    {
        _cellsTracker = GameService.GetService<ICellsTracker>();
        _cellsTracker.GetOnCellChange().AddListener(ChangeCurrentPosition);
        GetComponent<SpriteRenderer>().size = new Vector2(4f * 80, 3f * 60);
        //GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        //GetComponent<SpriteRenderer>().size = new Vector2(CellsMatrixData.CellSize.x, CellsMatrixData.CellSize.y);
    }

    public void ChangeCurrentPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    //  realization
    private ICellsTracker _cellsTracker;
}
