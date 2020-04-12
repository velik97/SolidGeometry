using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Stages.Actions
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class SetActiveShapeAction : ShapeAction
    {
        [JsonProperty]
        private bool m_Active;
        
        private bool m_PreviousState;

        public bool Active => m_Active;

        public SetActiveShapeAction(ShapeDataFactory shapeDataFactory) : base(shapeDataFactory)
        {
        }
        
        [JsonConstructor]
        public SetActiveShapeAction(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserialized();
        }

        public void SetIsActive(bool active)
        {
            if (m_Active == active)
            {
                return;
            }
            OnBecameDirty();
            m_Active = active;
        }

        public override void PreservePreviousState()
        {
            m_PreviousState = m_ShapeData?.View?.Active ?? false;
        }

        public override void ApplyAction()
        {
            if (m_ShapeData?.View != null)
            {
                m_ShapeData.View.Active = m_Active;
            }
        }

        public override void RollbackAction()
        {
            if (m_ShapeData?.View != null)
            {
                m_ShapeData.View.Active = m_PreviousState;
            }
        }

        public override string ToString()
        {
            return "Set " + (m_Active ? "active" : "not active") + " " + m_ShapeData?.ToString();
        }
    }
}