using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//  _objects schouldnt get access to movement simultaneously  
//  dependent from CardMovomentService 
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //  events
    public UnityEvent<PointerEventData> OnCardDragStart = new();
    public UnityEvent<PointerEventData> OnCardDrag = new();
    public UnityEvent<PointerEventData> OnCardDragEnd = new();
    public void Initialize() {
        Debug.Log("Drag handler init");
        try
        {
            movementService = GameService.GetService<CardMovementService>();
        }
        catch (Exception e)
        {
            Debug.LogError($"CardDragHandler init error: {e}");
        }
        if (movementService == null)
        {
            Debug.Log($"Error by card drag handler initialization");
            return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log($"Begin drag");
        movementService.SetCurrentCard(GetComponentInParent<RectTransform>());
        movementService.OnBeginDrag(eventData.position);
        OnCardDragStart.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        movementService.OnDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log($"end drag");
        OnCardDragEnd.Invoke(eventData);
        movementService.OnEndDrag(eventData.position);
    }

    //  
    private CardMovementService movementService;
}