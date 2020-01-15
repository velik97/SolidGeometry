using System;
using Editor.Shapes;
using Shapes.Blueprint;
using UnityEditor.UIElements;

namespace UnityEngine.UIElements
{
    public class PointBlueprintEditor : ShapeBlueprintEditor<PointBlueprint>
    {
        private TextField m_NameField;
        
        public PointBlueprintEditor(PointBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            TextField nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            Vector3Field positionField = new Vector3Field("Position") {value = Blueprint.PointData.Position};
            Toggle isAccessoryField = new Toggle("Is Accessory") {value = Blueprint.PointData.IsAccessoryPoint};

            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            positionField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.PointData.SetPosition(evt.newValue));
            isAccessoryField.RegisterCallback<ChangeEvent<bool>>(evt => Blueprint.PointData.SetIsAccessory(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(positionField);
            visualElement.Add(isAccessoryField);
        }

    }
}