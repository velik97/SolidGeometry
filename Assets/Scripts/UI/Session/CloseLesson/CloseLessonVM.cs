using System;
using Runtime.Core;
using UI.MVVM;
using Util.EventBusSystem;

namespace UI.Session.CloseLesson
{
    public class CloseLessonVM : ViewModel
    {
        private readonly Action m_CloseAction;

        public CloseLessonVM(Action closeAction)
        {
            m_CloseAction = closeAction;
        }

        public void CloseLesson()
        {
            m_CloseAction.Invoke();
        }
    }
}