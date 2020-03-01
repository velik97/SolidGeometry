using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stages
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

        public LessonStage CreateLessonStage()
        {
            LessonStage lessonStage = new LessonStage();
            m_LessonStages.Add(lessonStage);
            return lessonStage;
        }

        public void Remove(LessonStage lessonStage)
        {
            m_LessonStages.Remove(lessonStage);
        }

        public void Clear()
        {
            m_LessonStages.Clear();
        }
    }
}