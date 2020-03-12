using System;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;

namespace Lesson.Stages.Actions
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ShapeAction : CanDependOnShapeBlueprint
    {
        public event Action NameUpdated;
        public event Action ShapeDataUpdated;

        [JsonProperty]
        protected ShapeData m_ShapeData;

        private ShapeDataFactory m_ShapeDataFactory;

        public ShapeData ShapeData => m_ShapeData;

        public ShapeDataFactory ShapeDataFactory => m_ShapeDataFactory;

        protected ShapeAction(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
        }
        
        protected ShapeAction() 
        { }

        protected void OnDeserialized()
        {
            RestoreDependences();
            if (m_ShapeData != null)
            {
                m_ShapeData.NameUpdated += OnNameUpdated;
            }
        }

        public abstract void PreservePreviousState();

        public abstract void ApplyAction();

        public abstract void RollbackAction();

        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
        }

        public virtual void SetShapeData(ShapeData shapeData)
        {
            if (m_ShapeData == shapeData)
            {
                return;
            }
            if (m_ShapeData != null)
            {
                m_ShapeData.NameUpdated += OnNameUpdated;
            }
            m_ShapeData = shapeData;
            if (m_ShapeData != null)
            {
                m_ShapeData.NameUpdated += OnNameUpdated;
            }
            ShapeDataUpdated?.Invoke();
            OnNameUpdated();
        }

        public virtual bool HasConflictWith(ShapeAction other)
        {
            return other.m_ShapeData == this.m_ShapeData &&
                   other.GetType() == this.GetType();
        }

        protected void OnNameUpdated()
        {
            NameUpdated?.Invoke();
        }
    }
}