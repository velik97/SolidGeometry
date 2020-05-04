using System.Collections.Generic;
using Lesson.Stages;
using Runtime.Core;
using Runtime.Session;
using UI.MVVM;
using Util.EventBusSystem;

namespace UI.Session.LessonBrowserUI
{
    public class LessonBrowserVM : ViewModel
    {
        private GlobalData m_GlobalData;
        private readonly int m_MaxIndex;
        private int m_CurrentIndex;

        private readonly List<LessonStageDescriptionVM> m_StagesVMs = new List<LessonStageDescriptionVM>();
        public IReadOnlyList<LessonStageDescriptionVM> StagesVMs => m_StagesVMs;

        public LessonBrowserVM(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            foreach (LessonStage lessonStage in globalData.CurrentLessonData.LessonStageFactory.LessonStages)
            {
                LessonStageDescriptionVM stageVM = new LessonStageDescriptionVM(lessonStage);
                Add(stageVM);
                m_StagesVMs.Add(stageVM);
            }
        }

        public void GoToStage(int stageNumber)
        {
            m_GlobalData.SetCurrentLessonStageNumber(stageNumber);
        }
    }
}