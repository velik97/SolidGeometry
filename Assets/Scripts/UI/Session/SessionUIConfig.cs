using Session;
using UI.Session.LessonBrowserUI;
using UI.Session.LessonMovementUI;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.Session
{
    public class SessionUIConfig : MonoBehaviourCompositeDisposable
    {
        private SessionRunner m_SessionRunner;
        
        [SerializeField] private LessonBrowserView m_BrowserView;
        [SerializeField] private LessonMovementView m_LessonMovementView;
        
        public void Initialize(SessionRunner sessionRunner)
        {
            m_SessionRunner = sessionRunner;
            
            InitializeLessonBrowser();
            InitializeLessonMovement();
        }

        private void InitializeLessonBrowser()
        {
            LessonBrowserVM browserVM = new LessonBrowserVM(m_SessionRunner.LessonBrowser);
            Add(browserVM);
            m_BrowserView.Bind(browserVM);
        }

        private void InitializeLessonMovement()
        {
            LessonMovementVM movementVM = new LessonMovementVM(m_SessionRunner.LessonMovement);
            Add(movementVM);
            m_LessonMovementView.Bind(movementVM);
        }
    }
}