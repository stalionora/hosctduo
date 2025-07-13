using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

// depending from trailway model, GameService, ICellsTracker
// separately also implemented through plane, ray and local mouse position
public class CardMovementService: MonoBehaviour, IService, ICardMovementService
{ 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
                                                           //high of card while dragging
                                                           //use only for plane on drag
                                                           //or .z
    public UnityEvent OnCardDragBegin;
    public UnityEvent OnCardDragEnd;

    //
    public void Start() { 
    
        _mainCamera = Camera.allCameras[0];
        _canvasPosition = GetComponentInParent<Canvas>().transform.position;    //parents canvas transform
         //initialize
        _planeOfCanvas.SetNormalAndPosition(Vector3.forward, new Vector3(0, 0, _canvasPosition.z));
        _planeOnDrag.SetNormalAndPosition(Vector3.forward, new Vector3(0, 0, ZPointOnDrag));
        CalculateFructumRatio();
        OnCardDragBegin = new UnityEvent();
        OnCardDragEnd = new UnityEvent();
        //if (ZPointOfPlane > _canvasPosition.z)    //should be over the canvas
        //    ZPointOfPlane = _canvasPosition.z;
    }
    public void Initialize() 
    {
        _cellsTracker = GameService.GetService<ICellsTracker>();
        Assert.IsNotNull(_cellsTracker);
        Debug.Log("Cells initialization service");
    }
    //Events
    public void OnBeginDrag(Vector3 eventPosition)                 //
    {
        //  calculating point of the mouse 
        //    _lastValidPosition = transform.position;      
        _ray = _mainCamera.ScreenPointToRay(eventPosition);
        //_ray.direction = Vector3.forward;
        if (_planeOfCanvas.Raycast(_ray, out float enterDragPlaneRay) == true)  //  mouse offset calculation
        {
            _mouseOffset = (transform.position - _ray.GetPoint(enterDragPlaneRay));
            _mouseOffset.z = 0;
        }
        else
            throw new Exception("offset not determined");
        //  activating services, before they was starting to use in onDrag
        OnCardDragBegin.Invoke();
        //  takong up the card 
        transform.position = new Vector3(transform.position.x / _fructumRatio, transform.position.y / _fructumRatio, ZPointOnDrag);
        // 1! CellsTrackerService.Track(this)  //type = card
    }

    public void OnDrag(Vector3 eventPosition)  //
    {
        //SetDraggedPosition(eventData, ZPointOfPlane, _mouseOffset);
        _ray = _mainCamera.ScreenPointToRay(eventPosition);    //ray from cursor along forward?
        //_ray.direction = Vector3.forward;
        if (_planeOnDrag.Raycast(_ray, out float enterDragPlane) == true)    //interaction with a ray
        {
            //_cellsTracker.CalcuateCurrentCell(_ray.GetPoint(_canvasPosition.z));
            transform.position = _ray.GetPoint(enterDragPlane) + _mouseOffset;   //world point position
        }
        if (_planeOfCanvas.Raycast(_ray, out float enterCanvasPlane) == true)
        {
           _cellsTracker.CalcuateCurrentCell(_ray.GetPoint(enterCanvasPlane));
        }
    }

    public void OnEndDrag(Vector3 uselessPosition)
    {
        transform.position = _cellsTracker.GetCurrentCellCoordinates();
        OnCardDragEnd.Invoke();
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
    
    /// <summary>
    /// implementation shit
    /// </summary>
    private Camera _mainCamera;
    private Plane _planeOnDrag = new Plane();
    private Plane _planeOfCanvas = new Plane();
    private Ray _ray = new Ray();   //direct rays
    private Vector3 _canvasPosition;    //canvas surface
    private Vector3 _mouseOffset = new Vector3();   //offset before drag
    //  dependencies
    private ICellsTracker _cellsTracker;
    //
    private float _fructumRatio = 0.0f;
    private float ZPointOnDrag = 40.0f;
}
