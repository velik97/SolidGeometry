using System;
using Editor.Lesson.Blueprints.BaseShapes;
using Editor.Lesson.Blueprints.CompositeShapes;
using Editor.Lesson.Blueprints.DependentShapes;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.BaseShapes;
using Lesson.Shapes.Blueprints.CompositeShapes;
using Lesson.Shapes.Blueprints.DependentShapes;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints
{
    public static class ShapeBlueprintEditorFactory
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