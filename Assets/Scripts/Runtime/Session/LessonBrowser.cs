using System.Collections.Generic;
using Lesson;
using Lesson.Shapes.Datas;
using Lesson.Shapes.Views;
using Lesson.Stages;
using Runtime.Core;
using Runtime.Global;
using Runtime.Global.LessonManagement;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;

namespace Runtime.Session
{
    public class LessonBrowser : CompositeDisposable, ICurrentLessonStageNumberHandler
    {
        private readonly LessonStageFactory m_LessonStageFactory;
        
        private bool m_DefaultActiveState;
        private HighlightType m_DefaultHighlightType = HighlightType.Normal;

        private readonly Stack<LessonStage> m_AppliedActions = new Stack<LessonStage>();

        public LessonBrowser()
        {
            LessonData lessonData = LessonAccess.Instance.CurrentLessonData;
            m_LessonStageFactory = lessonData.LessonStageFactory;

            if (m_LessonStageFactory.LessonStages.Count > 0)
            {
                m_DefaultActiveState = false;
                ApplyDefaultState(lessonData.ShapeDataFactory);

                LessonStage firstStage = m_LessonStageFactory.LessonStages[0];
                m_AppliedActions.Push(firstStage);
                firstStage.ApplyActions();
                
                HandleLessonStageNumberChanged(LessonAccess.Instance.CurrentLessonStageNumber);
            }
            else
            {
                m_DefaultActiveState = true;
                ApplyDefaultState(lessonData.ShapeDataFactory);
            }
            
            Add(EventBus.Subscribe(this));
        }

        private void ApplyDefaultState(ShapeDataFactory shapeDataFactory)
        {
            foreach (ShapeData shapeData in shapeDataFactory.AllDatas)
            {
                shapeData.View.Active = m_DefaultActiveState;
                shapeData.View.Highlight = m_DefaultHighlightType;
            }
        }

        public void HandleLessonStageNumberChanged(int stageNumber)
        {
            if (stageNumber < 0 || stageNumber >= m_LessonStageFactory.LessonStages.Count)
            {
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