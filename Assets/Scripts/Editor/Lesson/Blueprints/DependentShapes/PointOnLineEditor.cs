using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.DependentShapes;
using Lesson.Shapes.Datas;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints.DependentShapes
{
    public class PointOnLineEditor : ShapeBlueprintEditor<PointOnLineBlueprint>
    {
        protected override string NameSuffix => "On Line";

        public PointOnLineEditor(PointOnLineBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
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
                "First Point: ",
                () => Blueprint.FirstPoint,
                pointData => Blueprint.SetFirstPoint(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Second point: ",
                () => Blueprint.SecondPoint,
                pointData => Blueprint.SetSecondPoint(pointData)));
            visualElement.Add(new ValidatorField(Blueprint.PointsNotSameValidator));

            FloatField coefField = new FloatField("Coefficient: ") {value = Blueprint.Coefficient};
            coefField.RegisterCallback<ChangeEvent<float>>(evt => Blueprint.SetCoefficient(evt.newValue));
            visualElement.Add(coefField);
        }
    }
}