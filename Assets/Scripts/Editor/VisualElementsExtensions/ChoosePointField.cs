using System;
using Shapes.Data;
using Shapes.Validators;
using Shapes.Validators.Line;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.VisualElementsExtensions
{
    public class ChoosePointField : VisualElement
    {
        private readonly ShapeDataFactory m_DataFactory;

        private readonly string m_FieldName;
        
        private readonly Func<PointData> m_GetPointFunc;
        private readonly Action<PointData> m_SetPointFunc;

        private readonly Label m_Label;
        private readonly ToolbarMenu m_ToolbarMenu;

        private Action m_UpdateValidatorAction;
        
        public ChoosePointField(ShapeDataFactory dataFactory, string fieldName, Func<PointData> getPointFunc, Action<PointData> setPointFunc)
        {
            m_DataFactory = dataFactory;
            m_FieldName = fieldName;
            m_GetPointFunc = getPointFunc;
            m_SetPointFunc = setPointFunc;

            VisualElement firstRow = new VisualElement {style = {flexDirection = FlexDirection.Row}};
            firstRow.Add(m_Label = new Label());
            firstRow.Add(m_ToolbarMenu = new ToolbarMenu {text = "Change"});
            Add(firstRow);

            IValidator pointNotEmptyValidator =
                new PointNotEmptyValidator(getPointFunc, action => m_UpdateValidatorAction = action);
            VisualElement validatorField = new ValidatorField(pointNotEmptyValidator);
            Add(validatorField);

            dataFactory.ShapesListUpdated += UpdateList;
            
            UpdateList();
            UpdateName();
            m_UpdateValidatorAction?.Invoke();
        }

        private void UpdateName()
        {
            m_Label.text = m_FieldName + " " + m_GetPointFunc()?.PointName;
        }

        private void UpdateList()
        {
            m_ToolbarMenu.menu.MenuItems().Clear();
            
            foreach (PointData pointData in m_DataFactory.PointDatas)
            {
                m_ToolbarMenu.menu.AppendAction(
                    pointData.ToString(), 
                    menuAction => SetPoint(pointData),
                    DropdownMenuAction.AlwaysEnabled);
            }
        }

        private void SetPoint(PointData pointData)
        {
            m_SetPointFunc?.Invoke(pointData);
            UpdateName();
            m_UpdateValidatorAction?.Invoke();
        }
    }
}