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
        [SerializeField] private string m_LessonFileName;
        // ================= For debug, should be taken from other place =================

        [SerializeField] private SessionUIConfig m_UIConfig;
        
        [SerializeField] private Transform m_SessionAnchor;

        private LessonData m_LessonData;

        private void Awake()
        {
            Initialize();
            
            InitializeShapeViewFactory();
            InitializeLessonBrowser();
            InitializeUI();
        }

        private void Initialize()
        {
            DontDestroyOnLoad(this);
            
            // ================= For debug, should be taken from other place =================
            FolderJsonsListSerializer<LessonData> serializer =
                new FolderJsonsListSerializer<LessonData>(Path.Combine(Application.dataPath, StaticPaths.FILES_FOLDER));
            
            m_LessonData = serializer.GetObject(m_LessonFileName);
            // ================= For debug, should be taken from other place =================
        }

        private void InitializeShapeViewFactory()
        {
            IShapeViewFactory shapeViewFactory = new ShapeViewFactoryProxy(ShapeViewFactory.Instance, m_SessionAnchor);
            m_LessonData.ShapeDataFactory.SetViewFactory(shapeViewFactory);
            Add(shapeViewFactory);
        } 

        private void InitializeLessonBrowser()
        {
            Add(new LessonBrowser(m_LessonData));
        }
        
        private void InitializeUI()
        {
            m_UIConfig.Initialize(m_LessonData);
            Add(m_UIConfig);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}