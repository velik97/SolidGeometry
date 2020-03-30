using System.Collections.Generic;
using Lesson.Stages;
using Session;
using UI.MVVM;

namespace UI.Session.LessonBrowserUI
{
    public class LessonBrowserVM : ViewModel
    {
        private readonly LessonBrowser m_LessonBrowser;

        private readonly int m_MaxIndex;
        private int m_CurrentIndex;

        private readonly List<LessonStageDescriptionVM> m_StagesVMs = new List<LessonStageDescriptionVM>();
        public IReadOnlyList<LessonStageDescriptionVM> StagesVMs => m_StagesVMs;

        public LessonBrowserVM(LessonBrowser browser)
        {
            m_LessonBrowser = browser;

            foreach (LessonStage lessonStage in browser.LessonStageFactory.LessonStages)
            {
                LessonStageDescriptionVM stageVM = new LessonStageDescriptionVM(lessonStage);
                Add(stageVM);
                m_StagesVMs.Add(stageVM);
            }
        }

        public void GoToStage(int stageNumber)
        {
            m_LessonBrowser.GoToStage(stageNumber);
        }
    }
}