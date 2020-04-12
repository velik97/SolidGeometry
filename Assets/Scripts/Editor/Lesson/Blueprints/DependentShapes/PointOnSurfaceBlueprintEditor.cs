using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.DependentShapes;
using Lesson.Shapes.Datas;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints.DependentShapes
{
    public class PointOnSurfaceBlueprintEditor : ShapeBlueprintEditor<PointOnSurfaceBlueprint>
    {
        public PointOnSurfaceBlueprintEditor(PointOnSurfaceBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
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
                "Source Point: ",
                () => Blueprint.SourcePoint,
                pointData => Blueprint.SetSourcePoint(pointData)));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Second point: ",
                () => Blueprint.SecondPoint,
                pointData => Blueprint.SetSecondPoint(pointData)));
            visualElement.Add(new ValidatorField(Blueprint.PointsNotSameValidator));
            visualElement.Add(new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                "Third point: ",
                () => Blueprint.ThirdPoint,
                pointData => Blueprint.SetThirdPoint(pointData)));
            
            FloatField coefficient1Field = new FloatField("Coefficient1: ") {value = Blueprint.Coefficient1};
            coefficient1Field.RegisterCallback<ChangeEvent<float>>(evt => Blueprint.SetCoefficient1(evt.newValue));
            visualElement.Add(coefficient1Field);
            
            FloatField coefficient2Field = new FloatField("Coefficient2: ") {value = Blueprint.Coefficient2};
            coefficient2Field.RegisterCallback<ChangeEvent<float>>(evt => Blueprint.SetCoefficient2(evt.newValue));
            visualElement.Add(coefficient2Field);
        }
    }
}