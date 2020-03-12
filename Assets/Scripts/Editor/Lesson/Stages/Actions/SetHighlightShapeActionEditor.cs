using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Datas;
using Lesson.Shapes.Views;
using Lesson.Stages.Actions;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson.Stages.Actions
{
    public class SetHighlightShapeActionEditor : ShapeActionEditor<SetHighlightShapeAction>
    {
        public SetHighlightShapeActionEditor(SetHighlightShapeAction shapeAction, Action<ShapeAction, VisualElement> deleteAction) : base(shapeAction, deleteAction)
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
            
            EnumField highlightField = new EnumField("Set Highlight") {value = ShapeAction.Highlight};
            highlightField.Init(ShapeAction.Highlight);
            highlightField.RegisterCallback<ChangeEvent<HighlightType>>(evt => ShapeAction.SetHighlightType(evt.newValue));
            
            visualElement.Add(highlightField);
        }
    }
}