using NUnit.Framework;
using UnityEngine;
////////////////////////////////////////////////////
//  trackt mouse position relative to some plane  //
////////////////////////////////////////////////////

class MouseTracker {
    //  should be used with trailways plane
    public MouseTracker(Vector3 planesOrigin) { 
        _mainCamera = Camera.main;
        //_mainCamera = Camera.allCameras[0];
        _plane.SetNormalAndPosition(Vector3.forward, planesOrigin);
    
    }
    //  distance between cursor and trailway's plane
    public Vector3 TrackMouse(Vector3 eventPosition,Camera camera = null) {
        return PerspectivePointTranslation(eventPosition);
    }
    //  distance between plane with between.z arbitrary point trailway's plane
    public Vector3 TrackMouse(Vector3 eventPosition, float distance,Camera camera = null) { 
        return PerspectivePointTranslation(eventPosition, distance);
    }
    //  distance between point and trailway's plane
    public Vector3 PerspectivePointTranslation(Vector3 rayStartPoint, float distance = 0) {
        _ray = _mainCamera.ScreenPointToRay(rayStartPoint);
        _plane.Raycast(_ray, out float enter);
        if (enter != 0)
        {
            return _ray.GetPoint(enter + distance);
        }
        else return new Vector3(float.MinValue, float.MinValue, float.MinValue);
    }
    //  distance between point and arbitrary plane
    public Vector3 PerspectivePointTranslation(Vector3 rayStartPoint, Plane plane, float distance = 0) {
        _ray = _mainCamera.ScreenPointToRay(rayStartPoint);
        Assert.AreNotEqual(plane, null);
        plane.Raycast(_ray, out float enter);
        if (enter != 0)
        {
            return _ray.GetPoint(enter + distance);
        }
        else return new Vector3(float.MinValue, float.MinValue, float.MinValue);
    }
////////////////////////////////////////////////////
    //  fields
    private Camera _mainCamera;
    private Plane _plane = new Plane();
    private Ray _ray = new Ray();   //direct rays
}