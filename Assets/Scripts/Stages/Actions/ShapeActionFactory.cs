using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stages.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShapeActionFactory
    {
        [JsonProperty]
        private List<ShapeAction> m_ShapeActions;

        public IReadOnlyList<ShapeAction> ShapeActions => m_ShapeActions;

        public ShapeActionFactory()
        {
            m_ShapeActions = new List<ShapeAction>();
        }
        
        [JsonConstructor]
        public ShapeActionFactory(object _)
        { }

        public ShapeAction CreateShapeAction(ShapeActionType shapeActionType)
        {
            ShapeAction shapeAction = null;
            switch (shapeActionType)
            {
                case ShapeActionType.SetActive:
                    shapeAction = new SetActiveShapeAction();
                    break;
                case ShapeActionType.SetHighlight:
                    shapeAction = new SetHighlightShapeAction();
                    break;
            }

            if (shapeAction != null)
            {
                m_ShapeActions.Add(shapeAction);
            }

            return shapeAction;
        }

        public void Remove(ShapeAction shapeAction)
        {
            m_ShapeActions.Remove(shapeAction);
        }

        public void Clear()
        {
            m_ShapeActions.Clear();
        }

        public enum ShapeActionType
        {
            SetActive,
            SetHighlight
        }
    }
}