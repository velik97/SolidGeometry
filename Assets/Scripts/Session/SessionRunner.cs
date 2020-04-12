using System;
using System.IO;
using Lesson;
using Lesson.Shapes.Views;
using Serialization;
using UI.Session;
using UnityEngine;
using Util.UniRxExtensions;

namespace Session
{
    public class SessionRunner : MonoBehaviourCompositeDisposable
    {
        // ================= For debug, should be taken from other place =================
        [SerializeField]
        private string m_LessonFileName;
        // ================= For debug, should be taken from other place =================

        [SerializeField]
        private SessionUIConfig m_UIConfig;
        [SerializeField]
        private Transform m_ShapesAnchor;
        [SerializeField]
        private Transform m_ShapesPivot;

        private LessonBrowser m_LessonBrowser;
        public LessonBrowser LessonBrowser => m_LessonBrowser;

        private LessonMovement m_LessonMovement;
        public LessonMovement LessonMovement => m_LessonMovement;

        private LessonData m_LessonData;
        public LessonData LessonData => m_LessonData;

        private void Awake()
        {
            Initialize();
            
            InitializeShapeViewFactory();
            InitializeLessonBrowser();
            InitializeLessonMovement();
            InitializeUI();
        }

        private void Initialize()
        {
            // ================= For debug, should be taken from other place =================
            FolderJsonsListSerializer<LessonData> serializer =
                new FolderJsonsListSerializer<LessonData>(Path.Combine(Application.dataPath, StaticPaths.FILES_FOLDER));
            
            m_LessonData = serializer.GetObject(m_LessonFileName);
            // ================= For debug, should be taken from other place =================
        }

        private void InitializeShapeViewFactory()
        {
            m_ShapesPivot.position = -LessonData.ShapeDataFactory.Origin;
            IShapeViewFactory shapeViewFactory = new ShapeViewFactoryProxy(ShapeViewFactory.Instance, m_ShapesPivot);
            m_LessonData.ShapeDataFactory.SetViewFactory(shapeViewFactory);
            Add(shapeViewFactory);
        } 

        private void InitializeLessonBrowser()
        {
            Add(m_LessonBrowser = new LessonBrowser(m_LessonData));
        }

        private void InitializeLessonMovement()
        {
            m_LessonMovement = new LessonMovement(m_ShapesAnchor);
        }
        
        private void InitializeUI()
        {
            m_UIConfig.Initialize(this);
            Add(m_UIConfig);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}