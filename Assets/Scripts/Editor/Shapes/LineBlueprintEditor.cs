using System;
using Shapes.Blueprint;
using Shapes.Data;
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
            m_StartPointChoose = new ToolbarMenu {text = "Change"};
            m_EndPointChoose = new ToolbarMenu {text = "Change"};

            m_StartPointLabel = new Label();
            m_EndPointLabel = new Label();
            
            VisualElement startPointScope = new VisualElement {style = {flexDirection = FlexDirection.Row}};
            startPointScope.Add(m_StartPointLabel);
            startPointScope.Add(m_StartPointChoose);
            
            VisualElement endPointScope = new VisualElement {style = {flexDirection = FlexDirection.Row}};
            endPointScope.Add(m_EndPointLabel);
            endPointScope.Add(m_EndPointChoose);

            visualElement.Add(startPointScope);
            visualElement.Add(endPointScope);
        }

        protected override void UpdateContent()
        {
            base.UpdateContent();
            
            m_StartPointChoose.menu.MenuItems().Clear();
            m_EndPointChoose.menu.MenuItems().Clear();

            m_StartPointLabel.text = $"Start point: {Blueprint.LineData.StartPoint?.PointName}";
            m_EndPointLabel.text = $"End point: {Blueprint.LineData.EndPoint?.PointName}";
            
            foreach (PointData pointData in Blueprint.PointDatas)
            {
                m_StartPointChoose.menu.AppendAction(
                    pointData.ToString(), 
                    menuAction => SetStartPoint(pointData),
                    DropdownMenuAction.AlwaysEnabled);
                
                m_EndPointChoose.menu.AppendAction(
                    pointData.ToString(), 
                    menuAction => SetEndPoint(pointData),
                    DropdownMenuAction.AlwaysEnabled);
            }
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