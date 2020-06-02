using System;
using Lesson.Shapes.Views;
using Runtime.Access.Lesson;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Lesson.Session3D
{
    public class Session3DRunner : MonoBehaviourMultipleDisposable, ISceneRunner
    {
        /// <summary>
        /// Contains m_LessonAnchor
        /// Represents actual center of lesson
        /// </summary>
        [SerializeField]
        private Transform m_LessonOrigin;

        /// <summary>
        /// Contains m_LessonAnchor
        /// This transforms is shifted, scaled and rotated responding on users input
        /// </summary>
        [SerializeField]
        private Transform m_LessonAnchor;
        
        /// <summary>
        /// Contains all shape views directly
        /// Shifted on minus lesson center so lessons center will be at (0,0,0)
        /// </summary>
        [SerializeField]
        private Transform m_LessonPivot;

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
            m_LessonPivot.localPosition = -LessonAccess.Instance.CurrentLessonData.ShapeDataFactory.Center;
            IShapeViewFactory shapeViewFactory = new ShapeViewFactoryProxy(ShapeViewFactory.Instance, m_LessonPivot);
            LessonAccess.Instance.CurrentLessonData.ShapeDataFactory.SetViewFactory(shapeViewFactory);
            AddDisposable(shapeViewFactory);
        } 

        private void InitializeLessonBrowser()
        {
            AddDisposable(m_LessonBrowser = new LessonBrowser());
        }

        private void InitializeLessonMovement()
        {
            AddDisposable(m_LessonMovement = new LessonMovement(m_LessonAnchor));
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}