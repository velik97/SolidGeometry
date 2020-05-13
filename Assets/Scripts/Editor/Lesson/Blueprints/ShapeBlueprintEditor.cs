using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints
{
    public abstract class ShapeBlueprintEditor<TBlueprint> : IHaveVisualElement where TBlueprint : ShapeBlueprint
    {
        protected readonly TBlueprint Blueprint;
        private readonly Action<ShapeBlueprint, VisualElement> m_DeleteAction;

        private Foldout m_NameElement;
        private Button m_DeleteButton;

        protected virtual string NameSuffix => "";

        protected ShapeBlueprintEditor(TBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction)
        {
            Blueprint = blueprint;
            m_DeleteAction = deleteAction;

            Blueprint.NameUpdated += UpdateName;
            Blueprint.DependenciesUpdated += UpdateDeleteButton;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            m_NameElement = new Foldout();
            m_NameElement.AddToClassList("sub-header-1");
            m_NameElement.Add(visualElement);

            m_NameElement.RegisterCallback<MouseEnterEvent>(_ => OnSelected());
            
            SetBaseVisualElement(visualElement);
            
            m_DeleteButton = new Button(() => m_DeleteAction(Blueprint, m_NameElement));
            m_DeleteButton.AddToClassList("delete");
            visualElement.Add(m_DeleteButton);

            UpdateName();
            UpdateDeleteButton();
            return m_NameElement;
        }

        private void OnSelected()
        {
            // Blueprint.MainShapeData.View?.SelectInEditor();
        }

        private void UpdateName()
        {
            m_NameElement.text = Blueprint.MainShapeData + "  " + NameSuffix;
        }

        private void UpdateDeleteButton()
        {
            m_DeleteButton.text = Blueprint.HaveDependencies ? "Can't delete, have dependencies" : "Delete";
            m_DeleteButton.SetEnabled(!Blueprint.HaveDependencies);
        }

        protected abstract void SetBaseVisualElement(VisualElement visualElement);
    }
}