using UnityEngine;

namespace Representation
{
    public class CircleDebugCellRepresentation : MonoBehaviour, ICellsRepresentation
    {
        [SerializeField]
        private UnityEngine.UI.Image SourceTexture;
        
        
        //
        public void Start()
        {
            _debugImageSource = SourceTexture;
        }


        // ICellsRepresentation
        public void SetRepresentationCollection(int amount) 
        {
            if(SourceTexture != null)
                SetDebugCollection(amount);
        }

        public void PlaceCellOnPoint(Vector3 pointToPlace) 
        {
            CellRepresentationOnPointDraw(pointToPlace);        
        }


        // own functionality
        public void SetDebugCollection(int amount) 
        {
            _imageCollection = new GameObject[amount];
            for (int i = 0; i < amount; ++i)
            {
                _imageCollection[i] = new GameObject();
                _imageCollection[i].name = i.ToString();
            }
            _counter = 0;
            //var shitTMP = ;
            //shitTMP.SetDebugImage(ref shitTMP);
        }
        public void CellRepresentationOnPointDraw(Vector3 point)
        {
            // 1. Переводим мировую точку в экранные координаты
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, point);
            // 2. Переводим экранные координаты в локальные координаты канваса
            RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, Camera.main, out localPoint);
            //
            var tmp = _imageCollection[_counter];   //next created object
            tmp.transform.SetParent(GameObject.Find("Trailground").transform, false);
            tmp.AddComponent<UnityEngine.UI.Image>().raycastTarget = false;
            var tmpRect = tmp.GetComponent<RectTransform>();
            // 3. Присваиваем локальные координаты
            tmpRect.anchoredPosition = localPoint;
            _imageCollection[_counter].GetComponent<UnityEngine.UI.Image>().sprite = _debugImageSource.sprite;
            // 4. log 
            Debug.Log(_counter + " " + _debugMessage + point + " --- " + tmpRect.anchoredPosition);
            ++_counter;
        }
        //public void SetDebugImage(ref CellDebugRepresentation  ptr) 
        //{
        //    _debugImageSource = ptr.SourceTexture;
        //}
        // realization 
        public void SetDebugImage(UnityEngine.UI.Image image)
        {
            _debugImageSource = image;
        }

        //
        private UnityEngine.UI.Image _debugImageSource;
        private GameObject[] _imageCollection;
        
        static private int _counter = 0;
        private string _debugMessage = "object have coordinates: ";
    } 
}