using System;
using System.IO;
using Lesson;
using Serialization;
using UnityEngine;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace Runtime.Core
{
    public class CoreRunner : MonoBehaviourCompositeDisposable
    {
        // ================= For debug, should be taken from other place =================
        [SerializeField]
        private string m_LessonFileName;
        // ================= For debug, should be taken from other place =================
        
        [SerializeField] private ApplicationConfig m_ApplicationConfig;

        private ApplicationModeManager m_ApplicationModeManager;

        private void Awake()
        {
            Initialize();
            InitializeLessonData();

            // Will be changed to main menu
            EventBus.RaiseEvent<IApplicationModeHandler>(h => h.HandleChangeApplicationMode(ApplicationMode.Session3D));
        }

        private void Initialize()
        {
            Add(m_ApplicationModeManager = new ApplicationModeManager(m_ApplicationConfig));
        }

        private void InitializeLessonData()
        {
            // ================= For debug, should be taken from other place =================
            FolderJsonsListSerializer<LessonData> serializer =
                new FolderJsonsListSerializer<LessonData>(Path.Combine(Application.dataPath, StaticPaths.FILES_FOLDER));
            
            LessonData lessonData = serializer.GetObject(m_LessonFileName);

            m_ApplicationModeManager.m_GlobalData.CurrentLessonData = lessonData;
            // ================= For debug, should be taken from other place =================
        }

        private void OnDisable()
        {
            Dispose();
        }
    }

    
}