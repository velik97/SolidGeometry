using System;
using JetBrains.Annotations;
using Shapes.Blueprint;
using Shapes.Data;
using Shapes.Validators;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.VisualElementsExtensions
{
    public class ChoosePointField : VisualElement
    {
        private readonly ShapeDataFactory m_DataFactory;
        private readonly ShapeBlueprint m_Blueprint;

        private readonly string m_FieldName;
        
        [NotNull] private readonly Func<PointData> m_GetPointFunc;
        [NotNull] private readonly Action<PointData> m_SetPointFunc;

        private readonly Label m_Label;
        private readonly ToolbarMenu m_ToolbarMenu;

        private Action m_UpdateValidatorAction;

        public ChoosePointField(ShapeBlueprint blueprint, string fieldName,
            Func<PointData> getPointFunc, Action<PointData> setPointFunc)
        {
            m_DataFactory = blueprint.DataFactory;
            m_Blueprint = blueprint;
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

            m_DataFactory.ShapesListUpdated += UpdateList;

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
            PointData previousPointData = m_GetPointFunc();
            if (pointData == previousPointData)
            {
                return;
            }

            if (!ShapeBlueprintDependencesCyclesSolver.CanCreateDependence(m_Blueprint, pointData))
            {
                Debug.LogError("Can't chose point because it will create a cycle in dependences");
                return;
            }
            
            if (previousPointData != null)
            {
                m_Blueprint.RemoveDependenceOn(previousPointData);
            }
            m_SetPointFunc.Invoke(pointData);
            if (pointData != null)
            {
                m_Blueprint.CreateDependenceOn(pointData);
            }
            UpdateName();
            m_UpdateValidatorAction?.Invoke();
        }
    }
}