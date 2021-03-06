using System;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Blueprints.BaseShapes;
using Lesson.Shapes.Datas;
using UnityEngine.UIElements;
using Util;

namespace Editor.Lesson.Blueprints.BaseShapes
{
    public class PolygonBlueprintEditor : ShapeBlueprintEditor<PolygonBlueprint>
    {
        private VisualElement m_PointsList;
        private Button m_DeleteButton;
        
        public PolygonBlueprintEditor(PolygonBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            m_PointsList = new VisualElement();
            visualElement.Add(m_PointsList);

            for (int i = 0; i < Blueprint.PolygonData.Points.Count; i++)
            {
                AddPointEditor(i);
            }
            
            visualElement.Add(new ValidatorField(Blueprint.PolygonData.PointsAreInOnePlaneValidator));
            visualElement.Add(new ValidatorField(Blueprint.PolygonData.PointsAreOnSameLineValidator));
            visualElement.Add(new ValidatorField(Blueprint.PolygonData.PointsNotSameValidator));
            visualElement.Add(new ValidatorField(Blueprint.PolygonData.LinesDontIntersectValidator));
            visualElement.Add(new ValidatorField(Blueprint.PolygonData.PolygonUniquenessValidator));

            VisualElement buttonsScope = new VisualElement {style = {flexDirection = FlexDirection.Row}};
            Button createNewPointButton = new Button(AddPoint) {text = "Add point"};
            createNewPointButton.AddToClassList("create");
            
            m_DeleteButton = new Button(RemoveLastPoint);
            m_DeleteButton.AddToClassList("delete");

            buttonsScope.Add(m_DeleteButton);
            buttonsScope.Add(createNewPointButton);
            
            visualElement.Add(buttonsScope);
            UpdateDeleteButton();
        }

        private void AddPoint()
        {
            Blueprint.PolygonData.AddPoint();
            AddPointEditor(Blueprint.PolygonData.Points.Count - 1);
            UpdateDeleteButton();
        }

        private void AddPointEditor(int index)
        {
            VisualElement setPointElement = new ChoseShapeDataField<PointData>(
                Blueprint.ShapeDataFactory,
                Blueprint,
                (index + 1).GetOrdinalForm() + " Point",
                () => Blueprint.PolygonData.Points[index],
                pointData => Blueprint.PolygonData.SetPoint(index, pointData));
            m_PointsList.Add(setPointElement);
        }

        private void RemoveLastPoint()
        {
            int index = Blueprint.PolygonData.Points.Count - 1;
            if (!Blueprint.PolygonData.CanRemovePoints)
            {
                return;
            }

            if (Blueprint.PolygonData.Points[index] != null)
            {
                Blueprint.RemoveDependenceOn(Blueprint.PolygonData.Points[index]);
            }
            
            Blueprint.PolygonData.RemovePoint(index);
            m_PointsList.RemoveAt(index);
            UpdateDeleteButton();
        }

        private void UpdateDeleteButton()
        {
            bool canRemovePoints = Blueprint.PolygonData.CanRemovePoints;
            m_DeleteButton.text = canRemovePoints ? "Delete point" : "Can't delete";
            m_DeleteButton.SetEnabled(canRemovePoints);
        }
    }
}