using Lesson.Shapes.Data;
using Lesson.Shapes.Views;
using UnityEngine;

namespace Lesson
{
    [RequireComponent(typeof(ShapeViewFactory))]
    public class LessonVisualizer : MonoBehaviour
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