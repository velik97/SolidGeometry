using Lesson.Shapes.Data;
using Lesson.Shapes.Views;

namespace Lesson.Stages.Actions
{
    public class SetHighlightShapeAction : ShapeAction
    {
        private HighlightType m_Highlight;
        
        private HighlightType m_PreviousState;

        public SetHighlightShapeAction(ShapeDataFactory shapeDataFactory) : base(shapeDataFactory)
        {
        }

        public void SetHighlightType(HighlightType highlight)
        {
            if (m_Highlight == highlight)
            {
                return;
            }
            m_Highlight = highlight;
            OnNameUpdated();
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