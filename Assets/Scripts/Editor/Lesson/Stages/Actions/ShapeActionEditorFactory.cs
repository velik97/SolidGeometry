using System;
using Lesson.Stages.Actions;
using UnityEngine.UIElements;

namespace Editor.Lesson.Stages.Actions
{
    public static class ShapeActionEditorFactory
    {
        public static VisualElement GetVisualElement(ShapeAction shapeAction,
            Action<ShapeAction, VisualElement> deleteAction)
        {
            VisualElement visualElement = null;
            switch (shapeAction)
            {
                case SetActiveShapeAction setActiveShapeAction:
                    visualElement = new SetActiveShapeActionEditor(setActiveShapeAction, deleteAction).GetVisualElement();
                    break;
                case SetHighlightShapeAction setHighlightShapeAction:
                    visualElement = new SetHighlightShapeActionEditor(setHighlightShapeAction, deleteAction).GetVisualElement();
                    break;
            }

            if (visualElement == null)
            {
                return null;
            }
            
            visualElement.AddToClassList("container");

            return visualElement;
        }
    }
}