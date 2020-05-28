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
    public class PyramidBlueprintEditor: ShapeBlueprintEditor<PyramidBlueprint>
    {
        private VisualElement m_PointsPositionsField;
        private VisualElement m_PointNameFields;

        private IntegerField m_VerticesCountField;
        private Label m_CantChangeVerticesCount = new Label("Cant change, have dependencies");
        
        public PyramidBlueprintEditor(PyramidBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
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

            Vector3Field topVertex = new Vector3Field("Top Vertex: ") {value = Blueprint.TopVertex};
            topVertex.RegisterCallback<ChangeEvent<Vector3>>(evt => Blueprint.SetOffset(evt.newValue));
            visualElement.Add(topVertex);
            
            Label positionsLabel = new Label("Points Positions");
            positionsLabel.AddToClassList("sub-header");
            visualElement.Add(positionsLabel);
            
            m_PointsPositionsField = new VisualElement();
            visualElement.Add(m_PointsPositionsField);
            
            Label namesLabel = new Label("Points Names");
            namesLabel.AddToClassList("sub-header");
            visualElement.Add(namesLabel);
            
            m_PointNameFields = new VisualElement();
            visualElement.Add(m_PointNameFields);

            Blueprint.DependenciesUpdated.Subscribe(UpdateVerticesCountFieldAvailability);
            UpdateVerticesCountFieldAvailability();
            UpdatePointNameFields();
            UpdatePointPositionsField();
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
            UpdatePointPositionsField();
        }

        private void UpdatePointPositionsField()
        {
            m_PointsPositionsField.Clear();

            for (int i = 0; i < Blueprint.PointsPositions.Count; i++)
            {
                int pointNum = i;
                Vector2Field positionField = new Vector2Field((pointNum + 1).GetOrdinalForm() + " Position: ")
                {
                    value = new Vector3(Blueprint.PointsPositions[i].x, Blueprint.PointsPositions[i].z)
                };
                positionField.RegisterCallback<ChangeEvent<Vector2>>(evt => 
                    Blueprint.SetPointPosition(pointNum, new Vector3(evt.newValue.x, 0, evt.newValue.y)));
                m_PointsPositionsField.Add(positionField);
            }
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