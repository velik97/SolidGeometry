using System;
using Editor.VisualElementsExtensions;
using Shapes.Blueprint.BaseShapes;
using Shapes.Data;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints.BaseShapes
{
    public class LineBlueprintEditor : ShapeBlueprintEditor<LineBlueprint>
    {
        private Label m_StartPointLabel;
        private Label m_EndPointLabel;

        private ToolbarMenu m_StartPointChoose;
        private ToolbarMenu m_EndPointChoose;
        
        public LineBlueprintEditor(LineBlueprint blueprint, Action<Shapes.Blueprint.ShapeBlueprint, VisualElement> deleteAction) : base(blueprint, deleteAction)
        {
        }

        protected override void SetBaseVisualElement(VisualElement visualElement)
        {
            ChoseShapeDataField<PointData> chooseStartPointField = new ChoseShapeDataField<PointData>(
                Blueprint.DataFactory,
                Blueprint,
                "Start Point",
                () => Blueprint.LineData.StartPoint,
                Blueprint.LineData.SetStartPoint);
            
            ChoseShapeDataField<PointData> chooseEndPointField = new ChoseShapeDataField<PointData>(
                Blueprint.DataFactory,
                Blueprint,
                "End Point",
                () => Blueprint.LineData.EndPoint,
                Blueprint.LineData.SetEndPoint);

            visualElement.Add(chooseStartPointField);
            visualElement.Add(chooseEndPointField);
            
            visualElement.Add(new ValidatorField(Blueprint.LineData.PointsNotSameValidator));
            visualElement.Add(new ValidatorField(Blueprint.LineData.UniquenessValidator));
        }
    }
}