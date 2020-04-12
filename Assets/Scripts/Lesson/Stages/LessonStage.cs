using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Stages.Actions;
using Lesson.Validators.LessonStages;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Stages
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LessonStage
    {
        public event Action BecameDirty;

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
        public LessonStage(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (ShapeAction shapeAction in m_ShapeActions)
            {
                shapeAction.BecameDirty += OnBecameDirty;
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
            BecameDirty?.Invoke();
        }

        public void SetName(string stageName)
        {
            m_StageName = stageName;
            BecameDirty?.Invoke();
        }

        public void SetDescription(string stageDescription)
        {
            m_StageDescription = stageDescription;
            BecameDirty?.Invoke();
        }

        public ShapeAction AddAction(ShapeActionFactory.ShapeActionType shapeActionType)
        {
            ShapeAction shapeAction = m_ShapeActionFactory.CreateShapeAction(shapeActionType);
            m_ShapeActions.Add(shapeAction);
            shapeAction.BecameDirty += OnBecameDirty;
            OnBecameDirty();
            return shapeAction;
        }

        public void RemoveAction(ShapeAction shapeAction)
        {
            m_ShapeActionFactory.Remove(shapeAction);
            m_ShapeActions.Remove(shapeAction);
            shapeAction.BecameDirty -= OnBecameDirty;
            OnBecameDirty();
        }

        public void ClearActions()
        {
            foreach (ShapeAction shapeAction in m_ShapeActions)
            {
                m_ShapeActionFactory.Remove(shapeAction);
                shapeAction.BecameDirty -= OnBecameDirty;
            }
            m_ShapeActions.Clear();
            OnBecameDirty();
        }

        public void ApplyActions()
        {
            List<ShapeAction> sorted = new List<ShapeAction>(m_ShapeActions);
            sorted.Sort();
            foreach (var action in sorted)
            {
                action.PreservePreviousState();
                action.ApplyAction();
            }		
        }

        public void RollbackActions()
        {
            List<ShapeAction> sorted = new List<ShapeAction>(m_ShapeActions);
            sorted.Sort();
            sorted.Reverse();
            foreach (var action in m_ShapeActions)
            {
                action.RollbackAction();
            }
        }

        private void OnBecameDirty()
        {
            BecameDirty?.Invoke();
        }
    }
}