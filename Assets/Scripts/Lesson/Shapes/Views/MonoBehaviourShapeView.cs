using Lesson.Shapes.Datas;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public abstract class MonoBehaviourShapeView<TShapeData> : MonoBehaviour, IShapeView where TShapeData : ShapeData
    {
        protected TShapeData ShapeData;
        
        public void SetShapeData(TShapeData shapeData)
        {
            if (ShapeData != null)
            {
                ShapeData.NameUpdated -= UpdateName;
                ShapeData.GeometryUpdated -= UpdateGeometry;
            }
            
            ShapeData = shapeData;
            
            if (ShapeData != null)
            {
                ShapeData.NameUpdated += UpdateName;
                ShapeData.GeometryUpdated += UpdateGeometry;
                
                UpdateName();
                UpdateGeometry();
            }
        }

        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        
        public abstract HighlightType Highlight { get; set; }
        
        public abstract void UpdateName();
        
        public abstract void UpdateGeometry();
    }
}