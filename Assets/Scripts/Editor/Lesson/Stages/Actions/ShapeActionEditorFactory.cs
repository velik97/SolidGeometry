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
            switch (shapeAction)
            {
                case SetActiveShapeAction setActiveShapeAction:
                    return new SetActiveShapeActionEditor(setActiveShapeAction, deleteAction).GetVisualElement();
                case SetHighlightShapeAction setHighlightShapeAction:
                    return new SetHighlightShapeActionEditor(setHighlightShapeAction, deleteAction).GetVisualElement();
            }

            return null;
        }
    }
}