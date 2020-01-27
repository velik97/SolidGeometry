using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Blueprints
{
    public class PointBlueprintEditor : ShapeBlueprintEditor<PointBlueprint>
    {
        public PointBlueprintEditor(PointBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            VisualElement nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            VisualElement positionField = new Vector3Field("Position") {value = Blueprint.PointData.Position};
            VisualElement isAccessoryField = new Toggle("Is Accessory") {value = Blueprint.PointData.IsAccessoryPoint};
            
            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            positionField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.PointData.SetPosition(evt.newValue));
            isAccessoryField.RegisterCallback<ChangeEvent<bool>>(evt => Blueprint.PointData.SetIsAccessory(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameNotEmptyValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameUniquenessValidator));
            visualElement.Add(positionField);
            visualElement.Add(new ValidatorField(Blueprint.PointData.PositionUniquenessValidator));
            visualElement.Add(isAccessoryField);
        }
    }
}