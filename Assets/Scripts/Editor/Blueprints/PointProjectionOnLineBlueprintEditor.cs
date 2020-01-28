using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint;
using Shapes.Blueprint.DependentShapes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Blueprints
{
    public class PointProjectionOnLineBlueprintEditor : ShapeBlueprintEditor<PointProjectionOnLineBlueprint>
    {
        public PointProjectionOnLineBlueprintEditor(PointProjectionOnLineBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            VisualElement nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            VisualElement isAccessoryField = new Toggle("Is Accessory") {value = Blueprint.PointData.IsAccessoryPoint};
            
            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            isAccessoryField.RegisterCallback<ChangeEvent<bool>>(evt => Blueprint.PointData.SetIsAccessory(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameNotEmptyValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameUniquenessValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.PositionUniquenessValidator));
            visualElement.Add(isAccessoryField);
            
            visualElement.Add(new ChoosePointField(
                Blueprint,
                "Source point: ",
                () => Blueprint.SourcePointData,
                pointData => Blueprint.SetSourcePoint(pointData)));
            visualElement.Add(new ChoosePointField(
                Blueprint,
                "First point on line: ",
                () => Blueprint.FirstPointOnLine,
                pointData => Blueprint.SetFirstPointOnLine(pointData)));
            visualElement.Add(new ChoosePointField(
                Blueprint,
                "Second point on line: ",
                () => Blueprint.SecondPointOnLine,
                pointData => Blueprint.SetSecondPointOnLine(pointData)));
        }
    }
}