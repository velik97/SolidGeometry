using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint;
using Shapes.Data;
using Shapes.Validators.Line;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Shapes
{
    public class LineBlueprintEditor : ShapeBlueprintEditor<LineBlueprint>
    {
        private Label m_StartPointLabel;
        private Label m_EndPointLabel;

        private ToolbarMenu m_StartPointChoose;
        private ToolbarMenu m_EndPointChoose;
        
        public LineBlueprintEditor(LineBlueprint blueprint, Action<ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            ChoosePointField chooseStartPointField = new ChoosePointField(
                Blueprint.DataFactory,
                "Start Point",
                () => Blueprint.LineData.StartPoint,
                SetStartPoint);
            
            ChoosePointField chooseEndPointField = new ChoosePointField(
                Blueprint.DataFactory,
                "End Point",
                () => Blueprint.LineData.EndPoint,
                SetEndPoint);

            visualElement.Add(chooseStartPointField);
            visualElement.Add(chooseEndPointField);
            
            visualElement.Add(new ValidatorField(Blueprint.LineData.m_PointsNotSameValidator));
            visualElement.Add(new ValidatorField(Blueprint.LineData.UniquenessValidator));
        }

        private void SetStartPoint(PointData pointData)
        {
            if (Blueprint.LineData.StartPoint == pointData)
            {
                return;
            }
            if (Blueprint.LineData.StartPoint != null)
            {
                Blueprint.RemoveDependenceOn(Blueprint.LineData.StartPoint);
            }
            Blueprint.LineData.SetStartPoint(pointData);
            
            if (Blueprint.LineData.StartPoint != null)
            {
                Blueprint.CreateDependenceOn(pointData);
            }
        }
        
        private void SetEndPoint(PointData pointData)
        {
            if (Blueprint.LineData.EndPoint == pointData)
            {
                return;
            }
            if (Blueprint.LineData.EndPoint != null)
            {
                Blueprint.RemoveDependenceOn(Blueprint.LineData.EndPoint);
            }
            Blueprint.LineData.SetEndPoint(pointData);
            
            if (Blueprint.LineData.EndPoint != null)
            {
                Blueprint.CreateDependenceOn(pointData);
            }
        }
    }
}