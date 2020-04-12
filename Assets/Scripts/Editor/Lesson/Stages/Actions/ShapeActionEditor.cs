using System;
using Editor.VisualElementsExtensions;
using Lesson.Stages.Actions;
using UnityEngine.UIElements;

namespace Editor.Lesson.Stages.Actions
{
    public abstract class ShapeActionEditor<TShapeAction> : IHaveVisualElement where TShapeAction : ShapeAction
    {
        protected readonly TShapeAction ShapeAction;
        private readonly Action<ShapeAction, VisualElement> m_DeleteAction;

        private Foldout m_NameElement;
        private Button m_DeleteButton;

        protected ShapeActionEditor(TShapeAction shapeAction, Action<ShapeAction, VisualElement> deleteAction)
        {
            ShapeAction = shapeAction;
            m_DeleteAction = deleteAction;

            ShapeAction.BecameDirty += UpdateName;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            m_NameElement = new Foldout {text = ShapeAction.ToString()};
            m_NameElement.Insert(0, visualElement);
            
            SetBaseVisualElement(visualElement);

            m_DeleteButton = new Button(() => m_DeleteAction(ShapeAction, m_NameElement)) {text = "Delete"};
            m_DeleteButton.AddToClassList("delete");

            visualElement.Add(m_DeleteButton);

            UpdateName();
            return m_NameElement;
        }

        private void UpdateName()
        {
            m_NameElement.text = ShapeAction.ToString();
        }

        protected abstract void SetBaseVisualElement(VisualElement visualElement);
    }
}