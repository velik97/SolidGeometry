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
        private Button m_DeleteButton;

        protected ShapeBlueprintEditor(TBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction)
        {
            Blueprint = blueprint;
            m_DeleteAction = deleteAction;

            Blueprint.NameUpdated += UpdateContent;
            Blueprint.DependencesUpdated += UpdateDeleteButton;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            m_NameElement = new Foldout {text = Blueprint.MainShapeData.ToString()};
            m_NameElement.Insert(0, visualElement);
            
            SetBaseVisualElement(visualElement);
            
            m_DeleteButton = new Button(() => m_DeleteAction(Blueprint, m_NameElement));
            visualElement.Add(m_DeleteButton);

            UpdateContent();
            UpdateDeleteButton();
            return m_NameElement;
        }

        protected virtual void UpdateContent()
        {
            m_NameElement.text = Blueprint.MainShapeData.ToString();
        }

        private void UpdateDeleteButton()
        {
            m_DeleteButton.text = Blueprint.HaveDependences ? "Can't delete" : "Delete";
            m_DeleteButton.SetEnabled(!Blueprint.HaveDependences);
        }

        protected abstract void SetBaseVisualElement(VisualElement visualElement);
    }
}