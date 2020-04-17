using System;
using System.IO;
using Lesson;
using Runtime;
using Runtime.Core;
using Serialization;
using UnityEngine;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace UI.MainMenu
{
    public class MainMenuUIRunner : MonoBehaviourCompositeDisposable, ISceneRunner
    {
        [SerializeField]
        private LessonsListView m_LessonsListView;
        
        private GlobalData m_GlobalData;

        private FolderJsonsListSerializer<LessonData> m_Serializer;
        
        public void Initialize(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            InitializeSerializer();
            InitializeLessonsList();
        }

        private void InitializeSerializer()
        {
             m_Serializer = new FolderJsonsListSerializer<LessonData>(Path.Combine(Application.dataPath, StaticPaths.FILES_FOLDER));
             m_Serializer.UpdateList();
        }

        private void InitializeLessonsList()
        {
            LessonListVM lessonListVM = new LessonListVM(m_Serializer.Names(), OnLessonChosen);
            Add(lessonListVM);
            m_LessonsListView.Bind(lessonListVM);
        }

        private void OnLessonChosen(string lessonName)
        {
            m_GlobalData.CurrentLessonData = m_Serializer.GetObject(lessonName);
            
            EventBus.RaiseEvent<IApplicationModeHandler>(
                h => h.HandleChangeApplicationMode(ApplicationMode.Session3D));
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}