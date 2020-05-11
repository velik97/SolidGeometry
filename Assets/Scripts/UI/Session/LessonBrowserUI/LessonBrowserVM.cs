using System.Collections.Generic;
using Lesson.Stages;
using Runtime.Core;
using Runtime.Global;
using Runtime.Global.LessonManagement;
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

        public LessonBrowserVM()
        {
            foreach (LessonStage lessonStage in LessonAccess.Instance.CurrentLessonData.LessonStageFactory.LessonStages)
            {
                LessonStageDescriptionVM stageVM = new LessonStageDescriptionVM(lessonStage);
                Add(stageVM);
                m_StagesVMs.Add(stageVM);
            }
        }

        public void GoToStage(int stageNumber)
        {
            LessonAccess.Instance.SetCurrentLessonStageNumber(stageNumber);
        }
    }
}