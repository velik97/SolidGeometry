using Newtonsoft.Json;
using Shapes.Blueprint;
using Shapes.Data;
using Stages;
using Stages.Actions;

namespace LessonComponents
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonData
    {
        [JsonProperty]
        private readonly ShapeDataFactory m_ShapeDataFactory;
        [JsonProperty]
        private readonly ShapeBlueprintFactory m_ShapeBlueprintFactory;
        [JsonProperty]
        private readonly LessonStageFactory m_LessonStageFactory;
        [JsonProperty]
        private readonly ShapeActionFactory m_ShapeActionFactory;

        public ShapeDataFactory ShapeDataFactory => m_ShapeDataFactory;
        public ShapeBlueprintFactory ShapeBlueprintFactory => m_ShapeBlueprintFactory;
        public LessonStageFactory LessonStageFactory => m_LessonStageFactory;
        public ShapeActionFactory ShapeActionFactory => m_ShapeActionFactory;

        public LessonData()
        {
            m_ShapeDataFactory = new ShapeDataFactory();
            m_ShapeBlueprintFactory = new ShapeBlueprintFactory(m_ShapeDataFactory);
        }

        [JsonConstructor]
        public LessonData(object _)
        { }
    }
}