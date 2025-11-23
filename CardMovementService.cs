using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

// depending from trailway model, GameService, ICellsTracker, CellsMatrixData, MovementWayService
// separately also implemented through plane, ray and local mouse position, mouseTracker
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
         _zPointOnDrag = _cellsMatrix.ZPointOnDrag;
        Debug.Log($"-------------> ZPOINT ON DRAG = {_zPointOnDrag}");
        _planeOfCanvas.SetNormalAndPosition(Vector3.forward, new Vector3(0, 0, _canvasPosition.z));
        _planeOnDrag.SetNormalAndPosition(Vector3.forward, new Vector3(0, 0, _zPointOnDrag));
        CalculateFructumRatio();
        //if (ZPointOfPlane > _canvasPosition.z)    //should be over the canvas
        //    ZPointOfPlane = _canvasPosition.z;
        _cellsTracker = GameService.GetService<ICellsTracker>();
        Assert.IsNotNull(_cellsTracker);
        _mouseTracker = new MouseTracker(new Vector3(0, 0, _canvasPosition.z));
        Debug.Log("Cells initialization service");
    }

    //public void SetCurrentCard(GameObject cardObjectWithRect) {
    //    _currentCard = cardObjectWithRect;   //  returns card in the hand, when card is thrown out of border of trailway 
    //    _originalPosition = _currentCard.GetComponent<RectTransform>().position;
    //    //Debug.Log($"original position = {_originalPosition}");
    //}
    //public GameObject GetCurrentCard() {
    //    return _currentCard;
    //}

    //  movement proceeding
    public void OnBeginDrag(PointerEventData eventData)                 //
    {
        if (_currentCard == null)
            Debug.LogError("No current card to move");
        //  calculating point of the mouse 
        _originalPosition = _currentCard.transform.position;
        _ray = _mainCamera.ScreenPointToRay(eventData.position);
        //_ray.direction = Vector3.forward;
        if (_planeOfCanvas.Raycast(_ray, out float enterDragPlaneRay) == true)  //  mouse offset calculation
        {
            _mouseOffset = (_currentCard.transform.position - _ray.GetPoint(enterDragPlaneRay));
            _mouseOffset.z = 0;
        }
        else
            throw new Exception("offset not determined");
        //  activating services, before they was starting to use in onDrag
        //  takong up the card 
        _currentCard.transform.position = new Vector3(_currentCard.transform.position.x / _fructumRatio, _currentCard.transform.position.y / _fructumRatio, _zPointOnDrag);
        // 1! CellsTrackerService.Track(this)  //type = card
    }

    public void OnDrag(PointerEventData eventData)  //
    {
        //Debug.Log($"Card dragging");
        //SetDraggedPosition(eventData, ZPointOfPlane, _mouseOffset);
        _ray = _mainCamera.ScreenPointToRay(eventData.position);    //ray from cursor along forward?
        //_ray.direction = Vector3.forward;
        if (_planeOnDrag.Raycast(_ray, out float enterDragPlane) == true)    //interaction with a ray
        {
            //_cellsTracker.CalcuateCurrentCell(_ray.GetPoint(_canvasPosition.z));
            _currentCard.transform.position = _ray.GetPoint(enterDragPlane) + _mouseOffset;   //world point position
        }
        if (_planeOfCanvas.Raycast(_ray, out float enterCanvasPlane) == true)
        {
           _cellsTracker.CalcuateCurrentCell(_ray.GetPoint(enterCanvasPlane));
        }
    }

    //  “”“  ¿–“¿ ƒŒÀ∆Õ¿ ”ƒ¿Àﬂ“‹—ﬂ
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_returnInHand)
            ReturnLastCardInTheHand();
        else
        {
            _currentCard.transform.position = _cellsTracker.GetCurrentCellCoordinates();
            //  !!!!!!! not optimized
            //_currentCard.gameObject.transform.position = _mouseTracker.PerspectivePointTranslation(_cellsTracker.GetCurrentCellCoordinates(), new Plane(Vector3.back, _zPointOnDrag));
        }
        //_currentCard.gameObject.transform.position = _cellsTracker.GetCurrentCellCoordinates();
    }

    public void ReturnLastCardInTheHand() {
        _currentCard.transform.position = _originalPosition;
    }

    private void CalculateFructumRatio()
    {
        if (_mainCamera.orthographic == false)
        {
            var canvasesFructumHeight = 2.0f * _canvasPosition.z * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var dragPlaneFructumHeight = 2.0f * _zPointOnDrag * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _fructumRatio = canvasesFructumHeight / dragPlaneFructumHeight;
        }
        else
            _fructumRatio = 1.0f;  
    }

    public void ReturnInHandOnEndDrag()
    {
        _returnInHand = true;
    }

    public void PlaceOnFieldOnEndDrag()
    {
        _returnInHand = false;
    }

    public void SetCurrentCard(ref GameObject currentCard) { 
        _currentCard = currentCard;
    }
    //  implementation shit
    //  mouse tracking related
    private Camera _mainCamera;
    private Plane _planeOnDrag = new Plane();
    private Plane _planeOfCanvas = new Plane();
    private Ray _ray = new Ray();   //direct rays
    private Vector3 _canvasPosition;    //canvas surface
    private Vector3 _mouseOffset = new Vector3();   //offset before drag
    //
    private MouseTracker _mouseTracker;
    //  card related
    private GameObject _currentCard;
    private Vector3 _originalPosition = new Vector3();   //position to return card before drag
    private bool _returnInHand = true;
    //  dependencies
    private ICellsTracker _cellsTracker;
    private CellsMatrixData _cellsMatrix;
    //
    private float _fructumRatio = 1.0f;
    private float _zPointOnDrag;
}
