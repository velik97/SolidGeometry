namespace Lesson.Stages.Actions
{
    public class SetActiveShapeAction : ShapeAction
    {
        private bool m_Active;
        
        private bool m_PreviousState;

        public bool Active => m_Active;

        public void SetIsActive(bool active)
        {
            if (m_Active == active)
            {
                return;
            }
            OnNameUpdated();
            m_Active = active;
        }

        public override void PreservePreviousState()
        {
            m_PreviousState = ShapeData?.View?.Active ?? false;
        }

        public override void ApplyAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Active = m_Active;
            }
        }

        public override void RollbackAction()
        {
            if (ShapeData?.View != null)
            {
                ShapeData.View.Active = m_PreviousState;
            }
        }

        public override string ToString()
        {
            return "Set " + (m_Active ? "active" : "not active") + " " + ShapeData?.ToString();
        }
    }
}