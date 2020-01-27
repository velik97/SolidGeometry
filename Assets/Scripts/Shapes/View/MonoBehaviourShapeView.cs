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
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public abstract void SetHighlight(HighlightType highlightType);
        
        public abstract void UpdateName(TShapeData shapeData);
        
        public abstract void UpdateGeometry(TShapeData shapeData);


        public void Release()
        {
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