using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.DependentShapes;
using Lesson.Shapes.Datas;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints.DependentShapes
{
    public class PointProjectionAlongLineBlueprintEditor : ShapeBlueprintEditor<PointProjectionAlongLineBlueprint>
    {
        public PointProjectionAlongLineBlueprintEditor(PointProjectionAlongLineBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            VisualElement nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            visualElement.Add(nameField);

            visualElement.Add(new ValidatorField(Blueprint.PointData.NameNotEmptyValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameUniquenessValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.PositionUniquenessValidator));

            // Projection point
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Projected point: ",
                () => Blueprint.ProjectedPoint,
                pointData => Blueprint.SetProjectedPoint(pointData)));
            
            // Target line
            Label targetLineLabel = new Label("Target Line");
            targetLineLabel.AddToClassList("sub-header");
            visualElement.Add(targetLineLabel);
            
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "1-st Point: ",
                () => Blueprint.FirstPointOnTargetLine,
                pointData => Blueprint.SetFirstPointOnTargetLine(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "2-nd Point: ",
                () => Blueprint.SecondPointOnTargetLine,
                pointData => Blueprint.SetSecondPointTargetOnLine(pointData)));
            visualElement.Add(new ValidatorField(Blueprint.TargetPointsNotSameValidator));
            
            // Parallel point
            Label parallelLineLabel = new Label("Parallel Line");
            parallelLineLabel.AddToClassList("sub-header");
            visualElement.Add(parallelLineLabel);

            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "1-st Point: ",
                () => Blueprint.FirstPointOnParallelLine,
                pointData => Blueprint.SetFirstPointOnParallelLine(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "2-nd Point: ",
                () => Blueprint.SecondPointOnParallelLine,
                pointData => Blueprint.SetSecondPointOnParallelLine(pointData)));
            visualElement.Add(new ValidatorField(Blueprint.ParallelPointsNotSameValidator));

            visualElement.Add(new ValidatorField(Blueprint.ProjectionAlongLineValidator));
        }
    }
}