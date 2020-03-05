using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Data;
using Lesson.Stages;
using Lesson.Stages.Actions;
using Newtonsoft.Json;

namespace Lesson
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonData
    {
        [JsonProperty]
        private readonly ShapeDataFactory m_ShapeDataFactory;
        [JsonProperty]
        private readonly ShapeBlueprintFactory m_ShapeBlueprintFactory;
        [JsonProperty]
        private readonly ShapeActionFactory m_ShapeActionFactory;
        [JsonProperty]
        private readonly LessonStageFactory m_LessonStageFactory;

        public ShapeDataFactory ShapeDataFactory => m_ShapeDataFactory;
        public ShapeBlueprintFactory ShapeBlueprintFactory => m_ShapeBlueprintFactory;
        public ShapeActionFactory ShapeActionFactory => m_ShapeActionFactory;
        public LessonStageFactory LessonStageFactory => m_LessonStageFactory;

        public LessonData()
        {
            m_ShapeDataFactory = new ShapeDataFactory();
            m_ShapeBlueprintFactory = new ShapeBlueprintFactory(m_ShapeDataFactory);
            
            m_ShapeActionFactory = new ShapeActionFactory(m_ShapeDataFactory);
            m_LessonStageFactory = new LessonStageFactory(m_ShapeActionFactory);
        }

        [JsonConstructor]
        public LessonData(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            m_ShapeBlueprintFactory.SetShapeDataFactory(m_ShapeDataFactory);
            m_ShapeActionFactory.SetShapeDataFactory(m_ShapeDataFactory);
            m_LessonStageFactory.SetShapeActionFactory(m_ShapeActionFactory);
        }
    }
}