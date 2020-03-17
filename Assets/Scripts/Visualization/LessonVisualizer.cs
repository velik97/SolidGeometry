using Lesson.Shapes.Datas;
using Lesson.Shapes.Views;
using UnityEngine;

namespace Visualization
{
    [RequireComponent(typeof(ShapeViewFactory))]
    public class LessonVisualizer : MonoBehaviour
    {
        private ShapeViewFactory m_ShapeViewFactory;

        private ShapeViewFactory ShapeViewFactory =>
            m_ShapeViewFactory ?? (m_ShapeViewFactory = GetComponent<ShapeViewFactory>());
        
        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            ShapeViewFactory.Dispose();
            shapeDataFactory.SetViewFactory(ShapeViewFactory);
        }
    }
}