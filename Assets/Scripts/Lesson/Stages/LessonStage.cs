using System;
using System.Collections.Generic;
using Lesson.Stages.Actions;
using Newtonsoft.Json;

namespace Lesson.Stages
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonStage
    {
        public event Action NameUpdated;
        public event Action NumUpdated;

        private int m_StageNum;
        [JsonProperty]
        private string m_StageName;
        [JsonProperty]
        private string m_StageDescription;

        private readonly List<ShapeAction> m_ShapeActions;

        public int StageNum => m_StageNum;
        public string StageName => m_StageName;
        public string StageDescription => m_StageDescription;

        public IReadOnlyList<ShapeAction> ShapeActions => m_ShapeActions;

        public LessonStage()
        {
            m_ShapeActions = new List<ShapeAction>();
        }

        public void SetNum(int num)
        {
            if (num == m_StageNum)
            {
                return;
            }
            m_StageNum = num;
            NumUpdated?.Invoke();
        }

        public void SetName(string stageName)
        {
            m_StageName = stageName;
            NameUpdated?.Invoke();
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