using UnityEngine;
using UnityEngine.EventSystems;

public interface IMovementService 
{
    void OnBeginDrag(PointerEventData eventPosition);
    void OnDrag(PointerEventData eventPosition);
    void OnEndDrag(PointerEventData eventPosition);
}