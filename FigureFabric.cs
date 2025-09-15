using NUnit.Framework;
using UnityEngine;

class FigureFabric : IFabric {
    public FigureFabric(GameObject prefab, RectTransform canvas = null) {
        Assert.IsNotNull(prefab);
        _prefab = prefab;
        _fieldCanvas = canvas;
    }
    public GameObject Create(Vector3 coordinates){
        GameObject newFigure;
        //
        if (_prefab == null)
            Debug.Log("null in fabric");
        //
        if (_fieldCanvas != null)
            newFigure = GameObject.Instantiate(_prefab, _fieldCanvas);
        else
            newFigure = GameObject.Instantiate(_prefab);
        ///////////////////////////////////////////////////////////////////////////////
        // -> -> -> _prefab.transform.localScale = scale of cells <- <- <-
        ///////////////////////////////////////////////////////////////////////////////
        
        //GameObject uiChild = new GameObject("UI_Image", typeof(RectTransform), typeof(CanvasRenderer), typeof(UnityEngine.UI.Image));
        
        //uiChild.transform.SetParent(newFigure.transform, false);

        //// ����������� RectTransform
        //RectTransform rt = uiChild.GetComponent<RectTransform>();
        //rt.anchorMin = Vector2.zero;
        //rt.anchorMax = Vector2.one;
        //rt.offsetMin = Vector2.zero;
        //rt.offsetMax = Vector2.zero;
        
        //// ����������� Image
        //UnityEngine.UI.Image img = uiChild.GetComponent<UnityEngine.UI.Image>();
        //img.raycastTarget = true;
        //img.color = Color.black;
        
        newFigure.SetActive(false);
        return newFigure;
    }
    private RectTransform _fieldCanvas;
    private GameObject _prefab;
}