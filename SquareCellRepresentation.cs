using UnityEngine;
using UnityEngine.Rendering;

namespace Representation
{
    class SquareCellRepresentation : MonoBehaviour, ICellsRepresentation
    {
        [SerializeField]
        private Sprite cellsSprite;
        [SerializeField]
        private CellsMatrixData cellsMatrixData;
        
        public void PlaceCellOnPoint(Vector3 point)
        {
            _collection[counter++].transform.position = point;
        }

        public void SetRepresentationCollection(int amount)
        {

            _collection = new GameObject[amount];
            var rotation = new Vector3(0, 0, 0);
            var parent = GameObject.Find("TrailwaysRepresentation");
            var sortingLayerId = parent.GetComponent<SortingGroup>().sortingLayerID;
            for (int i = 0; i < amount; ++i)
            {
                _collection[i] = new GameObject();
                _collection[i].transform.SetParent(parent.transform);
                var sprite = _collection[i].AddComponent<SpriteRenderer>();
                sprite.sortingLayerID = sortingLayerId;
                sprite.sortingOrder = 1;
                sprite.drawMode = SpriteDrawMode.Sliced; // allows resizing
                //sprite.size = new Vector2(cellsMatrixData.CellSize.x, cellsMatrixData.CellSize.y);    //makes nothings

                sprite.color = new Color(1f, 1f, 1f, 0.3f);
                sprite.sprite = cellsSprite;
                //_collection[i] = Sprite.Create(cellsSprite.texture, new Rect(), new Vector2());
                //_collection[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            }
        }

        private int counter = 0;
        private GameObject[] _collection;
    }
}