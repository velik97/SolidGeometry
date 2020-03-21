using Lesson.Shapes.Datas;
using Lesson.Shapes.Views;
using UnityEngine;

namespace Visualization
{
    [RequireComponent(typeof(ShapeViewFactory))]
    public class LessonVisualizer : MonoBehaviour
    {
        private IShapeViewFactory m_ViewFactory;

        private IShapeViewFactory ViewFactory =>
            m_ViewFactory ?? (m_ViewFactory = new ShapeViewFactoryProxy(ShapeViewFactory.Instance, transform));
        
        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            ViewFactory.CollectLostViews();
            ViewFactory.Dispose();
            shapeDataFactory.SetViewFactory(ViewFactory);
        }
    }
}