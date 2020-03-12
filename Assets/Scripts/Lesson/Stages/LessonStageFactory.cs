using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Stages.Actions;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Stages
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonStageFactory
    {
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
            return lessonStage;
        }

        public void Remove(LessonStage lessonStage)
        {
            lessonStage.ClearActions();
            m_LessonStages.Remove(lessonStage);
            for (var i = 0; i < m_LessonStages.Count; i++)
            {
                m_LessonStages[i].SetNum(i);
            }
        }

        public void Clear()
        {
            m_LessonStages.Clear();
        }
    }
}