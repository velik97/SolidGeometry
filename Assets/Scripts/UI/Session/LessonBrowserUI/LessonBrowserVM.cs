using System.Collections.Generic;
using Lesson.Stages;
using Runtime.Session;
using UI.MVVM;
using Util.EventBusSystem;

namespace UI.Session.LessonBrowserUI
{
    public class LessonBrowserVM : ViewModel
    {
        private readonly int m_MaxIndex;
        private int m_CurrentIndex;

        private readonly List<LessonStageDescriptionVM> m_StagesVMs = new List<LessonStageDescriptionVM>();
        public IReadOnlyList<LessonStageDescriptionVM> StagesVMs => m_StagesVMs;

        public LessonBrowserVM(IEnumerable<LessonStage> lessonStages)
        {
            foreach (LessonStage lessonStage in lessonStages)
            {
                LessonStageDescriptionVM stageVM = new LessonStageDescriptionVM(lessonStage);
                Add(stageVM);
                m_StagesVMs.Add(stageVM);
            }
        }

        public void GoToStage(int stageNumber)
        {
            EventBus.RaiseEvent<ILessonStageHandler>(h => h.HandleGoToStage(stageNumber));
        }
    }
}