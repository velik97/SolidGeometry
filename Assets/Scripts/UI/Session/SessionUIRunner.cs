using System;
using Lesson;
using Runtime;
using Runtime.Core;
using Runtime.Session;
using UI.Session.LessonBrowserUI;
using UI.Session.LessonMovementUI;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.Session
{
    public class SessionUIRunner : MonoBehaviourCompositeDisposable, ISceneRunner
    {
        [SerializeField] private LessonBrowserView m_BrowserView;
        [SerializeField] private LessonMovementView m_LessonMovementView;
        
        private GlobalData m_GlobalData;
        public void Initialize(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            InitializeLessonBrowser();
            InitializeLessonMovement();
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

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}