using System;
using Editor.Shapes;
using Shapes.Blueprint;
using UnityEditor.UIElements;

namespace UnityEngine.UIElements
{
    public class PointBlueprintEditor : ShapeBlueprintEditor<PointBlueprint>
    {
        private Label m_NonValidLabel;
        
        public PointBlueprintEditor(PointBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            TextField nameField = new TextField("Name") {value = Blueprint.PointData.PointName};
            Vector3Field positionField = new Vector3Field("Position") {value = Blueprint.PointData.Position};
            Toggle isAccessoryField = new Toggle("Is Accessory") {value = Blueprint.PointData.IsAccessoryPoint};
            
            m_NonValidLabel = new Label {style = { color = new StyleColor(Color.red)}};

            nameField.RegisterCallback<ChangeEvent<string>>(evt => Blueprint.PointData.SetName(evt.newValue));
            positionField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.PointData.SetPosition(evt.newValue));
            isAccessoryField.RegisterCallback<ChangeEvent<bool>>(evt => Blueprint.PointData.SetIsAccessory(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(m_NonValidLabel);
            visualElement.Add(positionField);
            visualElement.Add(isAccessoryField);
            
            UpdateNonValidLabel();
            Blueprint.PointData.NameUniquenessValidatable.UniquenessUpdated += UpdateNonValidLabel;
            Blueprint.PointData.PositionUniquenessValidatable.UniquenessUpdated += UpdateNonValidLabel;
            Blueprint.PointData.NameUpdated += UpdateNonValidLabel;
        }

        private void UpdateNonValidLabel()
        {
            if (string.IsNullOrEmpty(Blueprint.PointData.PointName))
            {
                m_NonValidLabel.text = "Name can't be empty";
                return;
            }

            if (!Blueprint.PointData.NameUniquenessValidatable.IsUnique)
            {
                m_NonValidLabel.text = $"Name '{Blueprint.PointData.PointName}' is not unique";
                return;
            }
            
            if (!Blueprint.PointData.PositionUniquenessValidatable.IsUnique)
            {
                m_NonValidLabel.text = $"Position is not unique";
                return;
            }

            m_NonValidLabel.text = string.Empty;
        }
    }
}