using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Stages.Actions;
using Lesson.Validators.LessonStages;
using Newtonsoft.Json;

namespace Lesson.Stages
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonStage
    {
        public event Action NameUpdated;
        public event Action ShapeActionsListUpdated;
        public event Action NumUpdated;

        private int m_StageNum;
        [JsonProperty]
        private string m_StageName;
        [JsonProperty]
        private string m_StageDescription;
        [JsonProperty]
        private readonly List<ShapeAction> m_ShapeActions;
        
        private ShapeActionFactory m_ShapeActionFactory;

        public NoConflictsBetweenShapeActionsValidator NoConflictsBetweenShapeActionsValidator;

        public int StageNum => m_StageNum;
        public string StageName => m_StageName;
        public string StageDescription => m_StageDescription;

        public IReadOnlyList<ShapeAction> ShapeActions => m_ShapeActions;

        public LessonStage(ShapeActionFactory shapeActionFactory)
        {
            m_ShapeActionFactory = shapeActionFactory;
            m_ShapeActions = new List<ShapeAction>();

            OnDeserialized();
        }
        
        [JsonConstructor]
        public LessonStage(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (ShapeAction shapeAction in m_ShapeActions)
            {
                shapeAction.ShapeDataUpdated += OnShapeActionsListUpdated;
            }
            OnDeserialized();
        }
        
        private void OnDeserialized()
        {
            NoConflictsBetweenShapeActionsValidator = new NoConflictsBetweenShapeActionsValidator(this);
        }

        public void SetShapeActionFactory(ShapeActionFactory shapeActionFactory)
        {
            m_ShapeActionFactory = shapeActionFactory;
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

        public ShapeAction AddAction(ShapeActionFactory.ShapeActionType shapeActionType)
        {
            ShapeAction shapeAction = m_ShapeActionFactory.CreateShapeAction(shapeActionType);
            m_ShapeActions.Add(shapeAction);
            shapeAction.ShapeDataUpdated += OnShapeActionsListUpdated;
            OnShapeActionsListUpdated();
            return shapeAction;
        }

        public void RemoveAction(ShapeAction shapeAction)
        {
            m_ShapeActionFactory.Remove(shapeAction);
            m_ShapeActions.Remove(shapeAction);
            shapeAction.ShapeDataUpdated -= OnShapeActionsListUpdated;
            OnShapeActionsListUpdated();
        }

        public void ClearActions()
        {
            foreach (ShapeAction shapeAction in m_ShapeActions)
            {
                m_ShapeActionFactory.Remove(shapeAction);
                shapeAction.ShapeDataUpdated -= OnShapeActionsListUpdated;
            }
            m_ShapeActions.Clear();
            OnShapeActionsListUpdated();
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

        protected virtual void OnShapeActionsListUpdated()
        {
            ShapeActionsListUpdated?.Invoke();
        }
    }
}