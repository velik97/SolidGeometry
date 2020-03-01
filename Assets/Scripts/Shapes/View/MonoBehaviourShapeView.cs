using Shapes.Data;
using UnityEngine;

namespace Shapes.View
{
    public abstract class MonoBehaviourShapeView<TShapeData> : MonoBehaviour, IShapeView where TShapeData : ShapeData
    {
        public void SetShapeData(TShapeData shapeData)
        {
            shapeData.NameUpdated += () => UpdateName(shapeData);
            shapeData.GeometryUpdated += () => UpdateGeometry(shapeData);

            UpdateName(shapeData);
            UpdateGeometry(shapeData);
        }

        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        
        public abstract HighlightType Highlight { get; set; }
        
        public abstract void UpdateName(TShapeData shapeData);
        
        public abstract void UpdateGeometry(TShapeData shapeData);


        public void Release()
        {
            if (gameObject == null)
            {
                return;
            }
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                DestroyImmediate(gameObject);
                return;
            }
#endif
            Destroy(gameObject);
        }
    }
}