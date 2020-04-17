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
        private CloseLessonView m_CloseLessonView;
        
        private GlobalData m_GlobalData;
        public void Initialize(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            InitializeLessonBrowser();
            InitializeLessonMovement();
            InitializeCloseLesson();
        }

        private void InitializeLessonBrowser()
        {
            LessonBrowserVM browserVM = new LessonBrowserVM(m_GlobalData.CurrentLessonData.LessonStageFactory.LessonStages);
            Add(browserVM);
            m_BrowserView.Bind(browserVM);
        }

        private void InitializeLessonMovement()
        {
            LessonMovementVM movementVM = new LessonMovementVM();
            Add(movementVM);
            m_LessonMovementView.Bind(movementVM);
        }

        private void InitializeCloseLesson()
        {
            CloseLessonVM closeLessonVM = new CloseLessonVM(CloseLesson);
            Add(closeLessonVM);
            m_CloseLessonView.Bind(closeLessonVM);
        }

        private void CloseLesson()
        {
            EventBus.RaiseEvent<IApplicationModeHandler>(h => h.HandleChangeApplicationMode(ApplicationMode.MainMenu));
        }
        

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}