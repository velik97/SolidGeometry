using System;
using Lesson;
using Lesson.Shapes.Views;
using Runtime.Core;
using Runtime.Global;
using Runtime.Global.LessonManagement;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Session.Session3D
{
    public class Session3DRunner : MonoBehaviourCompositeDisposable, ISceneRunner
    {
        [SerializeField]
        private Transform m_ShapesAnchor;
        [SerializeField]
        private Transform m_ShapesPivot;

        private LessonBrowser m_LessonBrowser;
        public LessonBrowser LessonBrowser => m_LessonBrowser;

        private LessonMovement m_LessonMovement;
        public LessonMovement LessonMovement => m_LessonMovement;

        public virtual void Initialize()
        {
            InitializeShapeViewFactory();
            InitializeLessonBrowser();
            InitializeLessonMovement();
        }

        private void InitializeShapeViewFactory()
        {
            m_ShapesPivot.localPosition = -LessonAccess.Instance.CurrentLessonData.ShapeDataFactory.Origin;
            IShapeViewFactory shapeViewFactory = new ShapeViewFactoryProxy(ShapeViewFactory.Instance, m_ShapesPivot);
            LessonAccess.Instance.CurrentLessonData.ShapeDataFactory.SetViewFactory(shapeViewFactory);
            Add(shapeViewFactory);
        } 

        private void InitializeLessonBrowser()
        {
            Add(m_LessonBrowser = new LessonBrowser());
        }

        private void InitializeLessonMovement()
        {
            Add(m_LessonMovement = new LessonMovement(m_ShapesAnchor));
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}