using System.Collections.Generic;
using Lesson;
using Lesson.Shapes.Datas;
using Lesson.Shapes.Views;
using Lesson.Stages;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;

namespace Session
{
    public class LessonBrowser : CompositeDisposable
    {
        private readonly LessonStageFactory m_LessonStageFactory;
        public LessonStageFactory LessonStageFactory => m_LessonStageFactory;

        private const bool DEFAULT_ACTIVE_STATE = false;
        private const HighlightType DEFAULT_HIGHLIGHT_TYPE = HighlightType.Normal;

        private readonly Stack<LessonStage> m_AppliedActions = new Stack<LessonStage>();

        public LessonBrowser(LessonData lessonData)
        {
            m_LessonStageFactory = lessonData.LessonStageFactory;
            
            ApplyDefaultState(lessonData.ShapeDataFactory);
            LessonStage firstStage = m_LessonStageFactory.LessonStages[0];
            m_AppliedActions.Push(firstStage);
            firstStage.ApplyActions();
        }

        private void ApplyDefaultState(ShapeDataFactory shapeDataFactory)
        {
            foreach (ShapeData shapeData in shapeDataFactory.AllDatas)
            {
                shapeData.View.Active = DEFAULT_ACTIVE_STATE;
                shapeData.View.Highlight = DEFAULT_HIGHLIGHT_TYPE;
            }
        }

        public void GoToStage(int stageNumber)
        {
            if (stageNumber < 0 || stageNumber >= m_LessonStageFactory.LessonStages.Count)
            {
                Debug.LogError($"Stage number {stageNumber} is out of range");
                return;
            }

            while (stageNumber >= m_AppliedActions.Count)
            {
                LessonStage stage = m_LessonStageFactory.LessonStages[m_AppliedActions.Count];
                m_AppliedActions.Push(stage);
                stage.ApplyActions();
            }

            while (stageNumber < m_AppliedActions.Count - 1)
            {
                m_AppliedActions.Pop().RollbackActions();
            }
        }
    }
}