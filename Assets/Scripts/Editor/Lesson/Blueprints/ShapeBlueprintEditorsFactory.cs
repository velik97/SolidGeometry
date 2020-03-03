using System;
using Editor.Blueprint;
using Editor.Blueprint.BaseShapes;
using Editor.Blueprint.CompositeShapes;
using Shapes.Blueprint.BaseShapes;
using Shapes.Blueprint.CompositeShapes;
using Shapes.Blueprint.DependentShapes;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints
{
    public static class ShapeBlueprintEditorsFactory
    {
        public static VisualElement GetVisualElement(Shapes.Blueprint.ShapeBlueprint blueprint,
            Action<Shapes.Blueprint.ShapeBlueprint, VisualElement> deleteAction)
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
                case CubeBlueprint cubeBlueprint:
                    return new CubeBlueprintEditor(cubeBlueprint, deleteAction).GetVisualElement();
                case PointProjectionOnLineBlueprint projectionOnLineBlueprint:
                    return new PointProjectionOnLineBlueprintEditor(projectionOnLineBlueprint, deleteAction).GetVisualElement();
                case PointOfIntersectionBlueprint pointOfIntersectionBlueprint:
                    return new PointOfIntersectionBlueprintEditor(pointOfIntersectionBlueprint, deleteAction).GetVisualElement();
            }

            return null;
        }
    }
}