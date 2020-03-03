using Lesson.Shapes.Views;

namespace Lesson.Stages.Actions
{
    public class SetHighlightShapeAction : ShapeAction
    {
        private HighlightType m_Highlight;
        
        private HighlightType m_PreviousState;

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
            m_PreviousState = ShapeData?.View?.Highlight ?? HighlightType.Normal;
        }

        public override void ApplyAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Highlight = m_Highlight;
            }
        }

        public override void RollbackAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Highlight = m_PreviousState;
            }
        }
        
        public override string ToString()
        {
            return "Set highlight " + m_Highlight + " " + ShapeData?.ToString();
        }
    }
}