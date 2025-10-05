using Unity.VisualScripting;
using UnityEngine;
////////////////////////////////////////////////////
//  trackt mouse position relative to some plane  //
////////////////////////////////////////////////////

class MouseTracker {
    public MouseTracker(Vector3 planesOrigin) { 
        _mainCamera = Camera.main;
        //_mainCamera = Camera.allCameras[0];
        _plane.SetNormalAndPosition(Vector3.forward, planesOrigin);
    
    }
    public Vector3 TrackMouse(Vector3 eventPosition,Camera camera = null) { 
        _ray = _mainCamera.ScreenPointToRay(eventPosition);
        _plane.Raycast(_ray, out float enter);
        if (enter != 0)
        {
            return _ray.GetPoint(enter);
        }
        else return new Vector3(float.MinValue, float.MinValue, float.MinValue);
    }
////////////////////////////////////////////////////
    //  fields
    private Camera _mainCamera;
    private Plane _plane = new Plane();
    private Ray _ray = new Ray();   //direct rays
}