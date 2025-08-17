using UnityEngine;

public interface IMovementService 
{
    void OnBeginDrag(Vector3 eventPosition);
    void OnDrag(Vector3 eventPosition);
    void OnEndDrag(Vector3 eventPosition);
}