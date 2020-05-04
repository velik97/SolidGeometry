using System;
using Lesson;
using Runtime;
using Runtime.Core;
using Runtime.Session;
using UI.Session.CloseLesson;
using UI.Session.LessonBrowserUI;
using UI.Session.LessonMovementUI;
using UnityEngine;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace UI.Session
{
    public class SessionUIRunner : MonoBehaviourCompositeDisposable, ISceneRunner
    {
        [SerializeField]
        private LessonBrowserView m_BrowserView;
        [SerializeField]
        private LessonMovementView m_LessonMovementView;

        [SerializeField]
        private TopPanelView m_TopPanelView;
        
        private GlobalData m_GlobalData;
        
        public void Initialize(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            InitializeLessonBrowser();
            InitializeLessonMovement();
            InitializeTopPanel();
        }

        private void InitializeLessonBrowser()
        {
            LessonBrowserVM browserVM = new LessonBrowserVM(m_GlobalData);
            Add(browserVM);
            m_BrowserView.Bind(browserVM);
        }

        private void InitializeLessonMovement()
        {
            LessonMovementVM movementVM = new LessonMovementVM();
            Add(movementVM);
            m_LessonMovementView.Bind(movementVM);
        }

        private void InitializeTopPanel()
        {
            TopPanelVM topPanelVM = new TopPanelVM(m_GlobalData);
            Add(topPanelVM);
            m_TopPanelView.Bind(topPanelVM);
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}