using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/////////////////////////////////////////////////////////////////////////////////////////////////////////
//  dependens on: cellsTracker
/////////////////////////////////////////////////////////////////////////////////////////////////////////
public class IndependentCellsTrackerTrigger : MonoBehaviour
{
    public UnityEvent<PointerEventData> OnClickCustom = new();
    public UnityEvent<Vector3> OnCursorMoveCustom = new();

    void Update() { 
        // calling cells tracker
        OnCursorMoveCustom.Invoke(Input.mousePosition);
        // interrupting cells tracking
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            Debug.Log("Stop making way event (fallback)");
            OnClickCustom.Invoke(data);
        }
    }

    

    private ICellsTracker _cellsTracker;
    //private MouseTracker _mouseTracker;
}