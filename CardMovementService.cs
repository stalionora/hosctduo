using System;
using UnityEngine;
using UnityEngine.Assertions;

// depending from trailway model, GameService, ICellsTracker, CellsMatrixData 
// separately also implemented through plane, ray and local mouse position
public class CardMovementService: IService, IMovementService
{ 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
                                                           //high of card while dragging
                                                           //use only for plane on drag
                                                           //or .z
    public CardMovementService(CellsMatrixData cellsMatrix){
        _cellsMatrix = cellsMatrix;
    }
    //
    public void Initialize() 
    {
        _mainCamera = Camera.allCameras[0];
        _canvasPosition = GameObject.Find($"{_cellsMatrix.ParentCanvas}").GetComponent<RectTransform>().position;    //parents canvas transform
         //initialize
        _planeOfCanvas.SetNormalAndPosition(Vector3.forward, new Vector3(0, 0, _canvasPosition.z));
        _planeOnDrag.SetNormalAndPosition(Vector3.forward, new Vector3(0, 0, ZPointOnDrag));
        CalculateFructumRatio();
        //if (ZPointOfPlane > _canvasPosition.z)    //should be over the canvas
        //    ZPointOfPlane = _canvasPosition.z;
        _cellsTracker = GameService.GetService<ICellsTracker>();
        Assert.IsNotNull(_cellsTracker);
        _cellsTracker.GetOnOutOfBorder().AddListener(ReturnInHand);
        _cellsTracker.GetOnCellChange().AddListener(PlaceOnFieldOnEndDrag);
        Debug.Log("Cells initialization service");
    }

    public void SetCurrentCard(RectTransform cardTransform) {
        _currentCard = cardTransform;   //  returns card in the hand, when card is thrown out of border of trailway 
        _originalPosition = _currentCard.position;
        Debug.Log($"original position = {_originalPosition}");
    }

    //  movement proceeding
    public void OnBeginDrag(Vector3 eventPosition)                 //
    {
        //  calculating point of the mouse 
        //    _lastValidPosition = transform.position;      
        _ray = _mainCamera.ScreenPointToRay(eventPosition);
        //_ray.direction = Vector3.forward;
        if (_planeOfCanvas.Raycast(_ray, out float enterDragPlaneRay) == true)  //  mouse offset calculation
        {
            _mouseOffset = (_currentCard.gameObject.transform.position - _ray.GetPoint(enterDragPlaneRay));
            _mouseOffset.z = 0;
        }
        else
            throw new Exception("offset not determined");
        //  activating services, before they was starting to use in onDrag
        //  takong up the card 
        _currentCard.gameObject.transform.position = new Vector3(_currentCard.gameObject.transform.position.x / _fructumRatio, _currentCard.gameObject.transform.position.y / _fructumRatio, ZPointOnDrag);
        // 1! CellsTrackerService.Track(this)  //type = card
    }

    public void OnDrag(Vector3 eventPosition)  //
    {
        Debug.Log($"Card dragging");
        //SetDraggedPosition(eventData, ZPointOfPlane, _mouseOffset);
        _ray = _mainCamera.ScreenPointToRay(eventPosition);    //ray from cursor along forward?
        //_ray.direction = Vector3.forward;
        if (_planeOnDrag.Raycast(_ray, out float enterDragPlane) == true)    //interaction with a ray
        {
            //_cellsTracker.CalcuateCurrentCell(_ray.GetPoint(_canvasPosition.z));
            _currentCard.gameObject.transform.position = _ray.GetPoint(enterDragPlane) + _mouseOffset;   //world point position
        }
        if (_planeOfCanvas.Raycast(_ray, out float enterCanvasPlane) == true)
        {
           _cellsTracker.CalcuateCurrentCell(_ray.GetPoint(enterCanvasPlane));
        }
    }

    //  “”“  ¿–“¿ ƒŒÀ∆Õ¿ ”ƒ¿Àﬂ“‹—ﬂ
    public void OnEndDrag(Vector3 uselessPosition)
    {
        Debug.Log($"in");
        if (_returnInHand)
            _currentCard.gameObject.transform.position = _originalPosition;
        else
            _currentCard.gameObject.transform.position = (Vector2)(_cellsTracker.GetCurrentCellCoordinates());
        //_currentCard.gameObject.transform.position = _cellsTracker.GetCurrentCellCoordinates();
    }

    private void CalculateFructumRatio()
    {
        if (_mainCamera.orthographic == false)
        {
            var canvasesFructumHeight = 2.0f * _canvasPosition.z * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var dragPlaneFructumHeight = 2.0f * ZPointOnDrag * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _fructumRatio = canvasesFructumHeight / dragPlaneFructumHeight;
        }
        else
            _fructumRatio = 1.0f;  
    }

    private void ReturnInHand()
    {
        _returnInHand = true;
    }

    private void PlaceOnFieldOnEndDrag(Vector3 point) {
        _returnInHand = false;
    }

    //  implementation shit
    //  mouse tracking related
    private Camera _mainCamera;
    private Plane _planeOnDrag = new Plane();
    private Plane _planeOfCanvas = new Plane();
    private Ray _ray = new Ray();   //direct rays
    private Vector3 _canvasPosition;    //canvas surface
    private Vector3 _mouseOffset = new Vector3();   //offset before drag
    //  card related
    private RectTransform _currentCard;
    private Vector3 _originalPosition = new Vector3();   //position to return card before drag
    private bool _returnInHand = true;
    //  dependencies
    private ICellsTracker _cellsTracker;
    private CellsMatrixData _cellsMatrix;
    //
    private float _fructumRatio = 1.0f;
    private float ZPointOnDrag = 40.0f;
}
