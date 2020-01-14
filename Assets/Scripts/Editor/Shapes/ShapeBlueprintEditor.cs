using System;
using Shapes.Blueprint;
using UnityEngine.UIElements;

namespace Editor.Shapes
{
    public abstract class ShapeBlueprintEditor<TBlueprint> where TBlueprint : ShapeBlueprint
    {
        protected readonly TBlueprint Blueprint;
        private readonly Action<ShapeBlueprint, VisualElement> m_DeleteAction;

        protected ShapeBlueprintEditor(TBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction)
        {
            Blueprint = blueprint;
            
            m_DeleteAction = deleteAction;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            Foldout foldout = new Foldout {text = Blueprint.MainShapeData.ToString()};
            foldout.Insert(0, visualElement);
            
            SetBaseVisualElement(visualElement);
            
            Button deleteButton = new Button(() => m_DeleteAction(Blueprint, foldout)) {text = "Delete"};
            visualElement.Add(deleteButton);

            return foldout;
        }

        protected abstract void SetBaseVisualElement(VisualElement visualElement);
    }
}