using System;
using Shapes.Blueprint;
using UnityEngine.UIElements;

namespace Editor.Shapes
{
    public static class ShapeBlueprintEditorsFactory
    {
        public static VisualElement GetVisualElement(ShapeBlueprint blueprint,
            Action<ShapeBlueprint, VisualElement> deleteAction)
        {
            switch (blueprint)
            {
                case PointBlueprint pointBlueprint:
                    return new PointBlueprintEditor(pointBlueprint, deleteAction).GetVisualElement();
                case LineBlueprint lineBlueprint:
                    return new LineBlueprintEditor(lineBlueprint, deleteAction).GetVisualElement();
                case PolygonBlueprint polygonBlueprint:
                    return new PolygonBlueprintEditor(polygonBlueprint, deleteAction).GetVisualElement();
            }

            return null;
        }
    }
}