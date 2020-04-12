using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.CompositeShapes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace Editor.Lesson.Blueprints.CompositeShapes
{
    public class RegularPrismBlueprintEditor: ShapeBlueprintEditor<RegularPrismBlueprint>
    {
        private VisualElement m_PointNameFields;

        private IntegerField m_VerticesCountField;
        private Label m_CantChangeVerticesCount = new Label("Cant change, have dependencies");

        public RegularPrismBlueprintEditor(RegularPrismBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }
        
        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            m_VerticesCountField = new IntegerField("Vertices Count: ") {value = Blueprint.VerticesAtTheBaseCount};
            m_VerticesCountField.RegisterCallback<ChangeEvent<int>>(evt => SetVerticesCount(evt.newValue));
            visualElement.Add(m_VerticesCountField);
            visualElement.Add(m_CantChangeVerticesCount);
        
            Vector3Field originField = new Vector3Field("Origin: ") {value = Blueprint.Origin};
            originField.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.SetOrigin(evt.newValue));
            visualElement.Add(originField);

            FloatField radiusField = new FloatField("Radius: ") {value = Blueprint.Radius};
            radiusField.RegisterCallback<ChangeEvent<float>>(evt => Blueprint.SetRadius(evt.newValue));
            visualElement.Add(radiusField);

            FloatField heightField = new FloatField("Height: ") {value = Blueprint.Height};
            heightField.RegisterCallback<ChangeEvent<float>>(evt => Blueprint.SetHeight(evt.newValue));
            visualElement.Add(heightField);

            // visualElement.Add(new ValidatorField(Blueprint.NonZeroVolumeValidator));

            m_PointNameFields = new VisualElement();
            visualElement.Add(m_PointNameFields);

            Blueprint.DependenciesUpdated += UpdateVerticesCountFieldAvailability;
            UpdateVerticesCountFieldAvailability();
            UpdatePointNameFields();
        }

        private void UpdateVerticesCountFieldAvailability()
        {
            m_VerticesCountField.SetEnabled(!Blueprint.HaveDependencies);
            m_CantChangeVerticesCount.visible = Blueprint.HaveDependencies;
        }

        private void SetVerticesCount(int count)
        {
            Blueprint.SetBaseVerticesCount(count);
            UpdatePointNameFields();
        }

        private void UpdatePointNameFields()
        {
            m_PointNameFields.Clear();
            
            for (int i = 0; i < Blueprint.Points.Count; i++)
            {
                int pointNum = i;
                TextField nameField = new TextField((pointNum + 1).GetOrdinalForm() + " Point name: ")
                    {value = Blueprint.Points[i].PointName};
                nameField.RegisterCallback<ChangeEvent<string>>(evt =>
                    Blueprint.SetPointName(pointNum, evt.newValue));
                m_PointNameFields.Add(nameField);
                m_PointNameFields.Add(new ValidatorField(Blueprint.Points[i].NameNotEmptyValidator));
                m_PointNameFields.Add(new ValidatorField(Blueprint.Points[i].NameUniquenessValidator));
                m_PointNameFields.Add(new ValidatorField(Blueprint.Points[i].PositionUniquenessValidator));
            }
        }
    }
}