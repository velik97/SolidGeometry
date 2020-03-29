using Session;
using UI.Session.LessonScrollBrowser;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.Session
{
    public class SessionUIConfig : MonoBehaviourCompositeDisposable
    {
        [SerializeField] private LessonBrowserView m_BrowserView;
        
        public void Initialize(SessionRunner sessionRunner)
        {
            LessonBrowserVM browserVM = new LessonBrowserVM(sessionRunner.LessonBrowser);
            Add(browserVM);
            m_BrowserView.Bind(browserVM);
        }
    }
}