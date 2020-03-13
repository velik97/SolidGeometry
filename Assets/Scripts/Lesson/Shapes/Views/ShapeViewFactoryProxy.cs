using System.Collections.Generic;
using Lesson.Shapes.Datas;
using UnityEngine;

namespace Lesson.Shapes.Views
{
    public class ShapeViewFactoryProxy : IShapeViewFactory
    {
        private readonly IShapeViewFactory m_InnerShapeViewFactory;
        private readonly Transform m_Transform;

        private readonly List<IShapeView> m_Views = new List<IShapeView>();

        public ShapeViewFactoryProxy(IShapeViewFactory innerShapeViewFactory, Transform transform)
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

        public void Clear()
        {
            while (m_Views.Count > 0)
            {
                ReleaseView(m_Views[m_Views.Count - 1]);
            }
        }
    }
}