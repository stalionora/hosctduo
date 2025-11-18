using JetBrains.Annotations;
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
    public UnityEvent<GameObject>       OnDetectingCurrentDraggedCard = new();
    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log($"Begin drag");
        OnDetectingCurrentDraggedCard.Invoke(transform.gameObject);
        OnCardDragStart.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        OnCardDrag.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {

        Debug.Log($"---------------------------------------END DRAG-------------------------------------------------");
        OnCardDragEnd.Invoke(eventData);
    }

}