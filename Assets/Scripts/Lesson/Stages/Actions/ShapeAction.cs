using System;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;

namespace Lesson.Stages.Actions
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ShapeAction : CanDependOnShapeBlueprint, IComparable<ShapeAction>
    {
        public event Action BecameDirty;

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
            RestoreDependencies();
            if (m_ShapeData != null)
            {
                m_ShapeData.NameUpdated += OnBecameDirty;
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
                m_ShapeData.NameUpdated -= OnBecameDirty;
            }
            m_ShapeData = shapeData;
            if (m_ShapeData != null)
            {
                m_ShapeData.NameUpdated += OnBecameDirty;
            }
            OnBecameDirty();
        }

        public virtual bool HasConflictWith(ShapeAction other)
        {
            return other.m_ShapeData == this.m_ShapeData &&
                   other.GetType() == this.GetType();
        }

        public int CompareTo(ShapeAction other)
        {
            bool thisHasCompositeShapeData = m_ShapeData is CompositeShapeData;
            bool otherHasCompositeShapeData = other.m_ShapeData is CompositeShapeData;

            if (thisHasCompositeShapeData == otherHasCompositeShapeData)
            {
                return 0;
            }

            if (thisHasCompositeShapeData)
            {
                return 1;
            }

            return -1;
        }

        protected void OnBecameDirty()
        {
            BecameDirty?.Invoke();
        }
    }
}