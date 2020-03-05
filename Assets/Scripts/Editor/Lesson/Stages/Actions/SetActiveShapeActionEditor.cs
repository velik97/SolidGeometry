using System;
using Editor.VisualElementsExtensions;
using Lesson.Stages.Actions;
using Shapes.Data;
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
            VisualElement choseShapeField = new ChoseShapeDataField<ShapeData>(
                ShapeAction.ShapeDataFactory,
                ShapeAction,
                "Chose Shape: ",
                () => ShapeAction.ShapeData,
                shapeData => ShapeAction.SetShapeData(shapeData));

            visualElement.Add(choseShapeField);
            
            Toggle setActiveField = new Toggle("Set Active: ") {value = ShapeAction.Active};
            setActiveField.RegisterCallback<ChangeEvent<bool>>(evt => ShapeAction.SetIsActive(evt.newValue));
            
            visualElement.Add(setActiveField);
        }
    }
}