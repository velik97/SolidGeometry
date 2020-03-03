using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Lesson.Stages
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonStageFactory
    {
        [JsonProperty]
        private readonly List<LessonStage> m_LessonStages;

        public IReadOnlyList<LessonStage> LessonStages => m_LessonStages;

        public LessonStageFactory()
        {
            m_LessonStages = new List<LessonStage>();
        }
        
        [JsonConstructor]
        public LessonStageFactory(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            for (var i = 0; i < m_LessonStages.Count; i++)
            {
                m_LessonStages[i].SetNum(i);
            }
        }

        public LessonStage CreateLessonStage()
        {
            LessonStage lessonStage = new LessonStage();
            m_LessonStages.Add(lessonStage);
            lessonStage.SetNum(m_LessonStages.Count - 1);
            return lessonStage;
        }

        public void Remove(LessonStage lessonStage)
        {
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