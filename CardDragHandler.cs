using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //  получть функцию передвижения
    public void SetMovementService(ICardMovementService service) 
    {
        _movementService = service;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _movementService.OnBeginDrag(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _movementService.OnBeginDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _movementService.OnBeginDrag(eventData.position);
    }

    //  objects schouldnt get access to function simultaneously  
    private ICardMovementService _movementService;
}