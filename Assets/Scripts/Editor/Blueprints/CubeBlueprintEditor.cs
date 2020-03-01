using System;
using Shapes.Blueprint;
using Shapes.Blueprint.Figures;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Blueprints
{
    public class CubeBlueprintEditor : ShapeBlueprintEditor<CubeBlueprint>
    {
        public CubeBlueprintEditor(CubeBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
        }
    }
}