using UnityEngine;
using UnityEngine.Rendering;
using Image = UnityEngine.UI.Image;

namespace Representation{
    public class SquareCellRepresentation : MonoBehaviour, ICellsRepresentation{
        [SerializeField]
        private float[] Color;
        //[SerializeField]
        [SerializeField]
        private float SpacingFactor;
        [SerializeField]
        private Sprite CellsSprite;
        [SerializeField]
        private CellsMatrixData CellsMatrixData;

        public void SetCellsTracker() {
            _cellsTracker = GameService.GetService<ICellsTracker>();
        }
        public void PlaceCellOnPoint(Vector3 point){
            _collection[counter++].transform.position = point;
        }

        public void SetRepresentationCollection(int amount){    //  initialize
            _collection = new GameObject[amount];
            var rotation = new Vector3(0, 0, 0);
            var sortingLayerId = GetComponent<SortingGroup>().sortingLayerID;
            _spacingRelativity = CellsMatrixData.CellSize.x / CellsMatrixData.CellSize.y;
            //var layer = _collection[sortingLayerId];
            for (int i = 0; i < amount; ++i){
                _collection[i] = new GameObject();
                _collection[i].transform.SetParent(transform);
                var rt = _collection[i].AddComponent<RectTransform>();
                var sprite = _collection[i].AddComponent<Image>();
                //sprite.sortingLayerID = sortingLayerId;
                //sprite.sortingLayerName = sortingLayerId;
                //sprite.sortingOrder = 1;
                //sprite.drawMode = SpriteDrawMode.Sliced; // allows resizing
                //sprite.mainTexture.width = (int)cellsMatrixData.CellSize.x;    //makes nothings
                //sprite.mainTexture.height = (int)cellsMatrixData.CellSize.y;    //makes nothings

                sprite.raycastTarget = false;
                rt.sizeDelta = new Vector2(CellsMatrixData.CellSize.x - SpacingFactor * _spacingRelativity, CellsMatrixData.CellSize.y - SpacingFactor);
                sprite.color = new Color(1f, 1f, 1f, 0.05f);
                sprite.sprite = CellsSprite;
                //_collection[i] = Sprite.Create(cellsSprite.texture, new Rect(), new Vector2());
                //_collection[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            }
            _stateBuf = _collection[_lastCellsNumber].GetComponent<Image>().color;
        }
        public void HighlightingElement(Vector3 useless = new Vector3()) {
            StopHighlightingLastElement();   //  optimise
            _lastCellsNumber = _cellsTracker.GetCurrentCellNumber();
            if (_lastCellsNumber > 0 || _lastCellsNumber < CellsMatrixData.Width * CellsMatrixData.Height){ 
                _stateBuf = _collection[_lastCellsNumber].GetComponent<Image>().color;
                _collection[_lastCellsNumber].GetComponent<Image>().color = new Color(0f, 100f, 200f, 0.4f);
            }
        }

        public void StopHighlightingLastElement(){
            _collection[_lastCellsNumber].GetComponent<Image>().color = _stateBuf;
        }
        
        private float _spacingRelativity = 0f; //  x - height 
        private int counter = 0;
        private int _lastCellsNumber = 0;
        
        private Color _stateBuf;
        private GameObject[] _collection;
        private ICellsTracker _cellsTracker;
    }
}

