using Lesson.Shapes.Datas;
using Lesson.Shapes.Views;
using UnityEngine;

namespace ConstructorVisualization
{
    public class LessonVisualizer : MonoBehaviour
    {
        private IShapeViewFactory m_ViewFactory;

        private IShapeViewFactory ViewFactory =>
            m_ViewFactory ?? (m_ViewFactory = new ShapeViewFactoryProxy(ShapeViewFactory.Instance, transform));

        private ShapeDataFactory m_ShapeDataFactory;

        private Vector3 m_LastOrigin = Vector3.positiveInfinity;
        private float m_LastTimeOriginChanged;
        
        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
            
            ViewFactory.CollectLostViews();
            ViewFactory.Dispose();
            m_ShapeDataFactory.SetViewFactory(ViewFactory);
            
            m_LastOrigin = Vector3.positiveInfinity;
        }

        private void OnDrawGizmos()
        {
            if (m_ShapeDataFactory == null)
            {
                return;
            }

            if (m_ShapeDataFactory.Origin != m_LastOrigin)
            {
                m_LastOrigin = m_ShapeDataFactory.Origin;
                m_LastTimeOriginChanged = Time.realtimeSinceStartup;
            }

            if (Time.realtimeSinceStartup - m_LastTimeOriginChanged > 5f)
            {
                return;
            }
            
            Color orange = new Color(1f, .5f, 0f);
            Gizmos.color = orange;
            
            Gizmos.DrawSphere(m_LastOrigin, .1f);
            Gizmos.DrawRay(m_LastOrigin, 4 * Vector3.up);
            Gizmos.DrawRay(m_LastOrigin, 4 * Vector3.down);
            Gizmos.DrawRay(m_LastOrigin, 4 * Vector3.right);
            Gizmos.DrawRay(m_LastOrigin, 4 * Vector3.left);
            Gizmos.DrawRay(m_LastOrigin, 4 * Vector3.forward);
            Gizmos.DrawRay(m_LastOrigin, 4 * Vector3.back);
        }
    }
}