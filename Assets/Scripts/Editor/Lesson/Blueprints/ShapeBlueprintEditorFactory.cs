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
            VisualElement visualElement = null;
            switch (blueprint)
            {
                case PointBlueprint pointBlueprint:
                    visualElement = new PointBlueprintEditor(pointBlueprint, deleteAction).GetVisualElement();
                    break;
                case LineBlueprint lineBlueprint:
                    visualElement = new LineBlueprintEditor(lineBlueprint, deleteAction).GetVisualElement();
                    break;
                case PolygonBlueprint polygonBlueprint:
                    visualElement = new PolygonBlueprintEditor(polygonBlueprint, deleteAction).GetVisualElement();
                    break;
                case ParallelepipedBlueprint parallelepipedBlueprint:
                    visualElement = new ParallelepipedBlueprintEditor(parallelepipedBlueprint, deleteAction).GetVisualElement();
                    break;
                case CubeBlueprint cubeBlueprint:
                    visualElement = new CubeBlueprintEditor(cubeBlueprint, deleteAction).GetVisualElement();
                    break;
                case PointPerpendicularProjectionBlueprint projectionOnLineBlueprint:
                    visualElement = new PointPerpendicularProjectionBlueprintEditor(projectionOnLineBlueprint, deleteAction).GetVisualElement();
                    break;
                case PointOfIntersectionBlueprint pointOfIntersectionBlueprint:
                    visualElement = new PointOfIntersectionBlueprintEditor(pointOfIntersectionBlueprint, deleteAction).GetVisualElement();
                    break;
                case PyramidBlueprint pyramidBlueprint:
                    visualElement = new PyramidBlueprintEditor(pyramidBlueprint, deleteAction).GetVisualElement();
                    break;
                case RegularPyramidBlueprint regularPyramidBlueprint:
                    visualElement = new RegularPyramidBlueprintEditor(regularPyramidBlueprint, deleteAction).GetVisualElement();
                    break;
                case RegularPrismBlueprint regularPrismBlueprint:
                    visualElement = new RegularPrismBlueprintEditor(regularPrismBlueprint, deleteAction).GetVisualElement();
                    break;
                case PointOnSurfaceBlueprint pointOnSurfaceBlueprint:
                    visualElement = new PointOnSurfaceBlueprintEditor(pointOnSurfaceBlueprint, deleteAction).GetVisualElement();
                    break;
                case PointOnLineBlueprint pointOnLineBlueprint:
                    visualElement = new PointOnLineEditor(pointOnLineBlueprint, deleteAction).GetVisualElement();
                    break;
                case PointProjectionAlongLineBlueprint pointProjectionAlongLineBlueprint:
                    visualElement = new PointProjectionAlongLineBlueprintEditor(pointProjectionAlongLineBlueprint, deleteAction).GetVisualElement();
                    break;
                case PointProjectionFromPointBlueprint pointProjectionFromPointBlueprint:
                    visualElement = new PointProjectionFromPointBlueprintEditor(pointProjectionFromPointBlueprint, deleteAction).GetVisualElement();
                    break;
            }
            if (visualElement == null)
            {
                return null;
            }
            
            visualElement.AddToClassList("container");

            return visualElement;
        }
    }
}