using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint.BaseShapes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints.BaseShapes
{
    public class PointBlueprintEditor : ShapeBlueprintEditor<PointBlueprint>
    {
        public PointBlueprintEditor(PointBlueprint blueprint, Action<Shapes.Blueprint.ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            VisualElement nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            VisualElement positionField = new Vector3Field("Position") {value = Blueprint.PointData.Position};
            
            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            positionField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.PointData.SetPosition(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameNotEmptyValidator));
            visualElement.Add(new ValidatorField(Blueprint.PointData.NameUniquenessValidator));
            visualElement.Add(positionField);
            visualElement.Add(new ValidatorField(Blueprint.PointData.PositionUniquenessValidator));
        }
    }
}