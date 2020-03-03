using Lesson.Shapes.Views;

namespace Lesson.Stages.Actions
{
    public class SetHighlightShapeAction : ShapeAction
    {
        public HighlightType Highlight;
        
        private HighlightType m_PreviousState;

        public override void PreservePreviousState()
        {
            m_PreviousState = ShapeData?.View?.Highlight ?? HighlightType.Normal;
        }

        public override void ApplyAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Highlight = Highlight;
            }
        }

        public override void RollbackAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Highlight = m_PreviousState;
            }
        }
    }
}