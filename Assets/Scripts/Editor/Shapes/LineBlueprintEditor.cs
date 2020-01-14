using System;
using Shapes.Blueprint;
using UnityEngine.UIElements;

namespace Editor.Shapes
{
    public class LineBlueprintEditor : ShapeBlueprintEditor<LineBlueprint>
    {
        public LineBlueprintEditor(LineBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            
        }
    }
}