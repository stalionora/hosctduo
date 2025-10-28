//  subscribe to on drag in cardmovementservice
//  check newPosition separately from cellstracker and make backlight on that cell
//  Depending only on icellstracker and cellsatrixdata

using UnityEngine;
using UnityEngine.EventSystems;

public class PositionIndicator: ReusableObject
{
    public PositionIndicator(CellsMatrixData cellsMatrixData, GameObject prefab,Transform parentCanvas)
    {
        _prefab = GameObject.Instantiate(prefab, parentCanvas, true);
        _cellsMatrixData = cellsMatrixData; 
        _prefab.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Initialize(Vector3? firstCellsCoordinates = null)
    {
        _cellsTracker = GameService.GetService<ICellsTracker>();
        ActivateTracking(null);
        Hide();
        //GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        //GetComponent<SpriteRenderer>().size = new Vector2(CellsMatrixData.CellSize.x, CellsMatrixData.CellSize.y);
    }
    public void ActivateTracking() {
        _cellsTracker.GetOnCellChange().AddListener(ChangeCurrentPosition);
        _cellsTracker.GetOnOutOfBorder().AddListener(Hide);
        _cellsTracker.GetOnOutOfBorder().AddListener(WaitForCellsTracker);
    }
    public void ActivateTracking(GameObject useless) {
        _cellsTracker.GetOnCellChange().AddListener(ChangeCurrentPosition);
        _cellsTracker.GetOnOutOfBorder().AddListener(Hide);
        _cellsTracker.GetOnOutOfBorder().AddListener(WaitForCellsTracker);
    }
    public void DeactivateTracking() {
        _cellsTracker.GetOnCellChange().RemoveListener(ChangeCurrentPosition);
        _cellsTracker.GetOnOutOfBorder().RemoveListener(Hide);
        _cellsTracker.GetOnOutOfBorder().RemoveListener(WaitForCellsTracker);
        _cellsTracker.GetOnCellChange().RemoveListener(Reset);
        _prefab.SetActive(false);
    }
    public void DeactivateTracking(GameObject useless) {
        DeactivateTracking();
    }

    public void Hide() {    // -> waitforcellstracker 
        _prefab.SetActive(false);
    }

    public void WaitActivationFromEvent(PointerEventData useless = null) {
        Hide();
        WaitForCellsTracker();
    }

    private void ChangeCurrentPosition(Vector3 newPosition)
    {
        _prefab.transform.position = newPosition;
    }

    private void Reset(Vector3 boundPosition)
    {
        if (_prefab.activeSelf)
            return;
        else
            _prefab.SetActive(true);
        ChangeCurrentPosition(boundPosition);
        _cellsTracker.GetOnCellChange().RemoveListener(Reset);
    }

    private void WaitForCellsTracker() {
        _cellsTracker.GetOnCellChange().AddListener(Reset);
    }


    //  realization
    private ICellsTracker _cellsTracker;
    private CellsMatrixData _cellsMatrixData;
    private GameObject _prefab;
}
