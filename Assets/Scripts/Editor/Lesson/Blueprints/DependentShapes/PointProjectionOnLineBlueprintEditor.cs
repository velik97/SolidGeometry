using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint.DependentShapes;
using Shapes.Data;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints.DependentShapes
{
    public class PointProjectionOnLineBlueprintEditor : ShapeBlueprintEditor<PointProjectionOnLineBlueprint>
    {
        public PointProjectionOnLineBlueprintEditor(PointProjectionOnLineBlueprint blueprint, Action<Shapes.Blueprint.ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
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
                Blueprint.DataFactory,
                Blueprint,
                "Source point: ",
                () => Blueprint.SourcePointData,
                pointData => Blueprint.SetSourcePoint(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.DataFactory,
                Blueprint,
                "First point on line: ",
                () => Blueprint.FirstPointOnLine,
                pointData => Blueprint.SetFirstPointOnLine(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.DataFactory,
                Blueprint,
                "Second point on line: ",
                () => Blueprint.SecondPointOnLine,
                pointData => Blueprint.SetSecondPointOnLine(pointData)));
            visualElement.Add(new ValidatorField(Blueprint.PointsNotSameValidator));
        }
    }
}