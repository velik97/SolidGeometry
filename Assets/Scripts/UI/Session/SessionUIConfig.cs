using Lesson;
using UI.Session.LessonBrowser;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.Session
{
    public class SessionUIConfig : MonoBehaviourCompositeDisposable
    {
        [SerializeField] private LessonBrowserView m_BrowserView;
        
        private LessonData m_LessonData;

        public void Initialize(LessonData lessonData)
        {
            m_LessonData = lessonData;

            LessonBrowserVM browserVM = new LessonBrowserVM(lessonData.LessonStageFactory);
            Add(browserVM);
            m_BrowserView.Bind(browserVM);
        }
    }
}