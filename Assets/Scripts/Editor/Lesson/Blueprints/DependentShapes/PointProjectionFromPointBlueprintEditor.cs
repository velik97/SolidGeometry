using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.DependentShapes;
using Lesson.Shapes.Datas;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

    //public class PointProjectionContinuationOfPoint


namespace Editor.Lesson.Blueprints.DependentShapes
{
    public class PointProjectionFromPointBlueprintEditor : ShapeBlueprintEditor<PointProjectionFromPointBlueprint>
    {
        public PointProjectionFromPointBlueprintEditor(PointProjectionFromPointBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
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
                "Projected Point: ",
                () => Blueprint.ProjectedPoint,
                pointData => Blueprint.SetProjectedPoint(pointData)));
            // Source point
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Source Point: ",
                () => Blueprint.SourcePoint,
                pointData => Blueprint.SetSourcePoint(pointData)));
            
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
                pointData => Blueprint.SetSecondPointOnTargetLine(pointData)));

            visualElement.Add(new ValidatorField(Blueprint.PointsNotSameValidator));


        }
    }
}
    