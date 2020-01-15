using System;
using Shapes.Blueprint;
using UnityEngine.UIElements;

namespace Editor.Shapes
{
    public abstract class ShapeBlueprintEditor<TBlueprint> where TBlueprint : ShapeBlueprint
    {
        protected readonly TBlueprint Blueprint;
        private readonly Action<ShapeBlueprint, VisualElement> m_DeleteAction;

        private Foldout m_NameElement;

        protected ShapeBlueprintEditor(TBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction)
        {
            Blueprint = blueprint;
            m_DeleteAction = deleteAction;

            Blueprint.NameUpdated += UpdateContent;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            m_NameElement = new Foldout {text = Blueprint.MainShapeData.ToString()};
            m_NameElement.Insert(0, visualElement);
            
            SetBaseVisualElement(visualElement);
            
            Button deleteButton = new Button(() => m_DeleteAction(Blueprint, m_NameElement)) {text = "Delete"};
            visualElement.Add(deleteButton);

            UpdateContent();
            return m_NameElement;
        }

        protected virtual void UpdateContent()
        {
            m_NameElement.text = Blueprint.MainShapeData.ToString();
        }

        protected abstract void SetBaseVisualElement(VisualElement visualElement);
    }
}