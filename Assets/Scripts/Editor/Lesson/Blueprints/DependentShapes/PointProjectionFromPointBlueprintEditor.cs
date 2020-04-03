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
            
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "First Point On Line: ",
                () => Blueprint.FirstPointOnLine,
                pointData => Blueprint.SetFirstPointOnLine(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Second point On Line: ",
                () => Blueprint.SecondPointOnLine,
                pointData => Blueprint.SetSecondPointOnLine(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Projected point: ",
                () => Blueprint.ProjectedPoint,
                pointData => Blueprint.SetProjectedPoint(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Point Along: ",
                () => Blueprint.PointAlong,
                pointData => Blueprint.SetPointAlong(pointData)));

            visualElement.Add(new ValidatorField(Blueprint.PointsNotSameValidator));


        }
    }
}
    