using Lesson;
using Lesson.Stages;
using Session;
using UI.MVVM;
using Util.EventBusSystem;

namespace UI.Session.LessonBrowser
{
    public class LessonBrowserVM : ViewModel
    {
        private readonly LessonStageFactory m_LessonStageFactory;

        private readonly int m_MaxIndex;
        private int m_CurrentIndex;

        public LessonBrowserVM(LessonStageFactory lessonStageFactory)
        {
            m_LessonStageFactory = lessonStageFactory;

            m_CurrentIndex = 0;
            m_MaxIndex = m_LessonStageFactory.LessonStages.Count;
        }

        public void ChoseNext()
        {
            if (m_CurrentIndex >= m_MaxIndex - 1)
            {
                return;
            }

            m_CurrentIndex++;
            
            EventBus.RaiseEvent<ILessonStageHandler>(h => h.HandleGoToStage(m_CurrentIndex));
        }

        public void ChosePrevious()
        {
            if (m_CurrentIndex < 1)
            {
                return;
            }

            m_CurrentIndex--;
            
            EventBus.RaiseEvent<ILessonStageHandler>(h => h.HandleGoToStage(m_CurrentIndex));
        }
    }
}