using System.Collections.Generic;
using Stages.Actions;

namespace Stages
{
    public class LessonStage
    {
        private string m_StageName;
        private string m_StageDescription;

        private readonly List<ShapeAction> m_ShapeActions;

        public string StageName => m_StageName;
        public string StageDescription => m_StageDescription;

        public IReadOnlyList<ShapeAction> ShapeActions => m_ShapeActions;

        public LessonStage()
        {
            m_ShapeActions = new List<ShapeAction>();
        }

        public void SetName(string stageName)
        {
            m_StageName = stageName;
        }

        public void SetDescription(string stageDescription)
        {
            m_StageDescription = stageDescription;
        }

        public void AddAction(ShapeAction shapeAction)
        {
            m_ShapeActions.Add(shapeAction);
        }

        public void RemoveAction(ShapeAction shapeAction)
        {
            m_ShapeActions.Remove(shapeAction);
        }

        public void ApplyActions()
        {
            foreach (var action in m_ShapeActions)
            {
                action.PreservePreviousState();
                action.ApplyAction();
            }		
        }

        public void RollbackActions()
        {
            foreach (var action in m_ShapeActions)
            {
                action.RollbackAction();
            }
        }
    }
}