namespace Lesson.Stages.Actions
{
    public class SetActiveShapeAction : ShapeAction
    {
        public bool Active;
        
        private bool m_PreviousState;

        public override void PreservePreviousState()
        {
            m_PreviousState = ShapeData?.View?.Active ?? false;
        }

        public override void ApplyAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Active = Active;
            }
        }

        public override void RollbackAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Active = m_PreviousState;
            }
        }
    }
}