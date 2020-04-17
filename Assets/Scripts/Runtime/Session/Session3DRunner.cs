using System;
using Lesson;
using Lesson.Shapes.Views;
using Runtime.Core;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Session
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

        private GlobalData m_GlobalData;
        private LessonData LessonData => m_GlobalData.CurrentLessonData;


        public void Initialize(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            InitializeShapeViewFactory();
            InitializeLessonBrowser();
            InitializeLessonMovement();
        }

        private void InitializeShapeViewFactory()
        {
            m_ShapesPivot.position = -LessonData.ShapeDataFactory.Origin;
            IShapeViewFactory shapeViewFactory = new ShapeViewFactoryProxy(ShapeViewFactory.Instance, m_ShapesPivot);
            LessonData.ShapeDataFactory.SetViewFactory(shapeViewFactory);
            Add(shapeViewFactory);
        } 

        private void InitializeLessonBrowser()
        {
            Add(m_LessonBrowser = new LessonBrowser(LessonData));
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