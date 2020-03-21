using System.Collections.Generic;
using System.Linq;
using Lesson.Shapes.Datas;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class ShapeViewFactoryProxy : IShapeViewFactory
    {
        private readonly ShapeViewFactory m_InnerShapeViewFactory;
        private readonly Transform m_Transform;

        private readonly List<IShapeView> m_Views = new List<IShapeView>();

        public ShapeViewFactoryProxy(ShapeViewFactory innerShapeViewFactory, Transform transform)
        {
            m_InnerShapeViewFactory = innerShapeViewFactory;
            m_Transform = transform;
        }

        public IShapeView RequestShapeView(ShapeData data)
        {
            IShapeView view = m_InnerShapeViewFactory.RequestShapeView(data);

            if (view is MonoBehaviour monoBehaviour)
            {
                monoBehaviour.transform.SetParent(m_Transform, false);
            }

            m_Views.Add(view);

            return view;
        }

        public void ReleaseView(IShapeView view)
        {
            m_Views.Remove(view);
            m_InnerShapeViewFactory.ReleaseView(view);
        }

        public void Dispose()
        {
            while (m_Views.Count > 0)
            {
                ReleaseView(m_Views[m_Views.Count - 1]);
            }
        }

        /// <summary>
        /// After recompiling project views might be lost
        /// </summary>
        public void CollectLostViews()
        {
            ClearNullViews();
            
            m_Views.AddRange(m_Transform.GetComponentsInChildren<IShapeView>().Where(view => !m_Views.Contains(view)));
            
            m_InnerShapeViewFactory.CollectLostViews();
        }

        private void ClearNullViews()
        {
            m_Views.RemoveAll(view => view == null);
        }
    }
}