using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Datas;
using Lesson.Stages;
using Lesson.Stages.Actions;
using Newtonsoft.Json;
using Serialization;

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

        public event Action DirtinessChanged;
        
        private bool m_IsDirty;

        public bool IsDirty => m_IsDirty;

        public LessonData()
        {
            m_ShapeDataFactory = new ShapeDataFactory();
            m_ShapeBlueprintFactory = new ShapeBlueprintFactory(m_ShapeDataFactory);
            
            m_ShapeActionFactory = new ShapeActionFactory(m_ShapeDataFactory);
            m_LessonStageFactory = new LessonStageFactory(m_ShapeActionFactory);
            
            OnDeserialized();
        }

        [JsonConstructor]
        public LessonData(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            m_ShapeBlueprintFactory.SetShapeDataFactory(m_ShapeDataFactory);
            m_ShapeActionFactory.SetShapeDataFactory(m_ShapeDataFactory);
            m_LessonStageFactory.SetShapeActionFactory(m_ShapeActionFactory);
            
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            m_ShapeDataFactory.BecameDirty.Subscribe(OnBecameDirty);
            m_LessonStageFactory.BecameDirty.Subscribe(OnBecameDirty);
        }

        private void OnBecameDirty()
        {
            m_IsDirty = true;
            DirtinessChanged?.Invoke();
        }

        public void Saved()
        {
            m_IsDirty = false;
            DirtinessChanged?.Invoke();
        }
    }
}