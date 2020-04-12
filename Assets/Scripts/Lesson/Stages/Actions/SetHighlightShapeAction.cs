using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Lesson.Shapes.Views;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Stages.Actions
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class SetHighlightShapeAction : ShapeAction
    {
        [JsonProperty]
        private HighlightType m_Highlight = HighlightType.Normal;
        
        private HighlightType m_PreviousState;

        public HighlightType Highlight => m_Highlight;

        public SetHighlightShapeAction(ShapeDataFactory shapeDataFactory) : base(shapeDataFactory)
        {
        }
        
        [JsonConstructor]
        public SetHighlightShapeAction(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserialized();
        }

        public void SetHighlightType(HighlightType highlight)
        {
            if (m_Highlight == highlight)
            {
                return;
            }
            m_Highlight = highlight;
            OnBecameDirty();
        }

        public override void PreservePreviousState()
        {
            m_PreviousState = m_ShapeData?.View?.Highlight ?? HighlightType.Normal;
        }

        public override void ApplyAction()
        {
            if (m_ShapeData?.View != null)
            {
                m_ShapeData.View.Highlight = m_Highlight;
            }
        }

        public override void RollbackAction()
        {
            if (m_ShapeData?.View != null)
            {
                m_ShapeData.View.Highlight = m_PreviousState;
            }
        }
        
        public override string ToString()
        {
            return "Set highlight " + m_Highlight + " " + m_ShapeData?.ToString();
        }
    }
}