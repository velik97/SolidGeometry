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
                Blueprint,
                "Start Point",
                () => Blueprint.LineData.StartPoint,
                Blueprint.LineData.SetStartPoint);
            
            ChoosePointField chooseEndPointField = new ChoosePointField(
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