using Shapes.Data;
using Shapes.View;
using UnityEngine;

namespace Visualization
{
    [RequireComponent(typeof(ShapeViewFactory))]
    public class FiguresSetVisualizer : MonoBehaviour
    {
        private ShapeViewFactory m_ShapeViewFactory;

        private ShapeViewFactory ShapeViewFactory =>
            m_ShapeViewFactory ?? (m_ShapeViewFactory = GetComponent<ShapeViewFactory>());
        
        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            ShapeViewFactory.Clear();
            shapeDataFactory.SetViewFactory(ShapeViewFactory);
        }
    }
}