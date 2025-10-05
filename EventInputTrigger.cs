using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//  dependens on: cellsTracker
/////////////////////////////////////////////////////////////////////////////////////////////////////////
public class EventInputTrigger : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent<PointerEventData> OnClickCustom = new();
    public UnityEvent<Vector3> OnCursorMoveCustom = new();

    void Update() { 
        OnCursorMoveCustom.Invoke(Input.mousePosition);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickCustom.Invoke(eventData);
    }

    private ICellsTracker _cellsTracker;
    //private MouseTracker _mouseTracker;
}