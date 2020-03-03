using System;
using Lesson.Stages.Actions;
using UnityEngine.UIElements;

namespace Editor.Lesson.Stages.Actions
{
    public class SetActiveShapeActionEditor : ShapeActionEditor<SetActiveShapeAction>
    {
        public SetActiveShapeActionEditor(SetActiveShapeAction shapeAction, Action<ShapeAction, VisualElement> deleteAction) : base(shapeAction, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            return;
        }
    }
}