using System.Collections.Generic;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Stages.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShapeActionFactory
    {
        [JsonProperty]
        private List<ShapeAction> m_ShapeActions;

        private ShapeDataFactory m_ShapeDataFactory;

        public IReadOnlyList<ShapeAction> ShapeActions => m_ShapeActions;

        public ShapeActionFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
            m_ShapeActions = new List<ShapeAction>();
        }
        
        [JsonConstructor]
        public ShapeActionFactory(JsonConstructorMark _)
        { }

        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            m_ShapeDataFactory = shapeDataFactory;
            foreach (ShapeAction shapeAction in m_ShapeActions)
            {
                shapeAction.SetShapeDataFactory(m_ShapeDataFactory);
            }
        }

        public ShapeAction CreateShapeAction(ShapeActionType shapeActionType)
        {
            ShapeAction shapeAction = null;
            switch (shapeActionType)
            {
                case ShapeActionType.SetActive:
                    shapeAction = new SetActiveShapeAction(m_ShapeDataFactory);
                    break;
                case ShapeActionType.SetHighlight:
                    shapeAction = new SetHighlightShapeAction(m_ShapeDataFactory);
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
            shapeAction.Destroy();
            m_ShapeActions.Remove(shapeAction);
        }

        public void Clear()
        {
            foreach (ShapeAction shapeAction in m_ShapeActions)
            {
                shapeAction.Destroy();
            }
            m_ShapeActions.Clear();
        }

        public enum ShapeActionType
        {
            SetActive,
            SetHighlight
        }
    }
}