using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Stages.Actions;
using Newtonsoft.Json;
using Serialization;
using Util.CascadeUpdate;

namespace Lesson.Stages
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonStageFactory
    {
        public CascadeUpdateEvent BecameDirty = new CascadeUpdateEvent();

        [JsonProperty]
        private readonly List<LessonStage> m_LessonStages;
        
        private ShapeActionFactory m_ShapeActionFactory;

        public IReadOnlyList<LessonStage> LessonStages => m_LessonStages;

        public LessonStageFactory(ShapeActionFactory shapeActionFactory)
        {
            m_ShapeActionFactory = shapeActionFactory;
            m_LessonStages = new List<LessonStage>();
        }
        
        [JsonConstructor]
        public LessonStageFactory(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            for (var i = 0; i < m_LessonStages.Count; i++)
            {
                m_LessonStages[i].SetNum(i);
                m_LessonStages[i].BecameDirty += OnBecameDirty;
            }
        }

        public void SetShapeActionFactory(ShapeActionFactory shapeActionFactory)
        {
            m_ShapeActionFactory = shapeActionFactory;
            foreach (LessonStage lessonStage in m_LessonStages)
            {
                lessonStage.SetShapeActionFactory(m_ShapeActionFactory);
            }
        }

        public LessonStage CreateLessonStage()
        {
            LessonStage lessonStage = new LessonStage(m_ShapeActionFactory);
            m_LessonStages.Add(lessonStage);
            lessonStage.SetNum(m_LessonStages.Count - 1);
            lessonStage.BecameDirty += OnBecameDirty;

            return lessonStage;
        }

        public void Remove(LessonStage lessonStage)
        {
            lessonStage.ClearActions();
            m_LessonStages.Remove(lessonStage);
            lessonStage.BecameDirty -= OnBecameDirty;

            for (var i = 0; i < m_LessonStages.Count; i++)
            {
                m_LessonStages[i].SetNum(i);
            }
        }

        public void SwapStages(LessonStage lessonStage, bool up)
        {
            int stageIndex = m_LessonStages.IndexOf(lessonStage);
            int swapStageIndex = stageIndex + (up ? -1 : 1);

            if (swapStageIndex < 0 || swapStageIndex >= m_LessonStages.Count)
            {
                return;
            }

            LessonStage tmp = m_LessonStages[stageIndex];
            m_LessonStages[stageIndex] = m_LessonStages[swapStageIndex];
            m_LessonStages[swapStageIndex] = tmp;
                
            for (var i = 0; i < m_LessonStages.Count; i++)
            {
                m_LessonStages[i].SetNum(i);
            }
        }

        public void Clear()
        {
            foreach (LessonStage lessonStage in m_LessonStages)
            {
                lessonStage.BecameDirty -= OnBecameDirty;
            }

            m_LessonStages.Clear();
        }
        
        private void OnBecameDirty()
        {
            BecameDirty?.Invoke();
        }
    }
}