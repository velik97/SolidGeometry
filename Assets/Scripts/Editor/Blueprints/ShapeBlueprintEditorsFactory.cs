using System;
using Editor.Blueprints.BaseShapes;
using Editor.Blueprints.Figures;
using Shapes.Blueprint;
using Shapes.Blueprint.BaseShapes;
using Shapes.Blueprint.DependentShapes;
using Shapes.Blueprint.Figures;
using UnityEngine.UIElements;

namespace Editor.Blueprints
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
                case ParallelepipedBlueprint parallelepipedBlueprint:
                    return new ParallelepipedBlueprintEditor(parallelepipedBlueprint, deleteAction).GetVisualElement();
                case PointProjectionOnLineBlueprint projectionOnLineBlueprint:
                    return new PointProjectionOnLineBlueprintEditor(projectionOnLineBlueprint, deleteAction).GetVisualElement();
            }

            return null;
        }
    }
}