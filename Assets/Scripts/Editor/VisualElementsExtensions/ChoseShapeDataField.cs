using System;
using JetBrains.Annotations;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Data;
using Lesson.Validators;
using Shapes.Blueprint;
using Shapes.Data;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.VisualElementsExtensions
{
    public class ChoseShapeDataField<TShapeData> : VisualElement where TShapeData : ShapeData
    {
        private readonly ShapeDataFactory m_DataFactory;
        private readonly CanDependOnShapeBlueprint m_Dependent;

        private readonly string m_FieldName;
        
        [NotNull] private readonly Func<TShapeData> m_GetShapeDataFunc;
        [NotNull] private readonly Action<TShapeData> m_SetShapeDataFunc;

        private readonly Label m_Label;
        private readonly ToolbarMenu m_ToolbarMenu;

        private Action m_UpdateValidatorAction;

        public ChoseShapeDataField(ShapeDataFactory shapeDataFactory, CanDependOnShapeBlueprint dependent,
            string fieldName, Func<TShapeData> getShapeDataFunc, Action<TShapeData> setShapeDataFunc)
        {
            m_DataFactory = shapeDataFactory;
            m_Dependent = dependent;
            m_FieldName = fieldName;
            m_GetShapeDataFunc = getShapeDataFunc;
            m_SetShapeDataFunc = setShapeDataFunc;

            VisualElement firstRow = new VisualElement {style = {flexDirection = FlexDirection.Row}};
            firstRow.Add(m_Label = new Label());
            firstRow.Add(m_ToolbarMenu = new ToolbarMenu {text = "Change"});
            Add(firstRow);

            IValidator pointNotEmptyValidator =
                new DataNotEmptyValidator<TShapeData>(getShapeDataFunc, action => m_UpdateValidatorAction = action);
            VisualElement validatorField = new ValidatorField(pointNotEmptyValidator);
            Add(validatorField);

            m_DataFactory.ShapesListUpdated += UpdateList;

            UpdateList();
            UpdateName();
            m_UpdateValidatorAction?.Invoke();
        }

        private void UpdateName()
        {
            m_Label.text = m_FieldName + " " + m_GetShapeDataFunc()?.ToString();;
        }

        private void UpdateList()
        {
            m_ToolbarMenu.menu.MenuItems().Clear();
            
            foreach (TShapeData pointData in m_DataFactory.GetShapeDatasList<TShapeData>())
            {
                m_ToolbarMenu.menu.AppendAction(
                    pointData.ToString(), 
                    menuAction => SetData(pointData),
                    DropdownMenuAction.AlwaysEnabled);
            }
        }

        private void SetData(TShapeData shapeData)
        {
            TShapeData previousPointData = m_GetShapeDataFunc();
            if (shapeData == previousPointData)
            {
                return;
            }

            if (!CanDependOnShapeBlueprint.CanCreateDependence(m_Dependent, shapeData))
            {
                Debug.LogError("Can't chose data because it will create a cycle in dependences");
                return;
            }
            
            if (previousPointData != null)
            {
                m_Dependent.RemoveDependenceOn(previousPointData);
            }
            m_SetShapeDataFunc.Invoke(shapeData);
            if (shapeData != null)
            {
                m_Dependent.CreateDependenceOn(shapeData);
            }
            UpdateName();
            m_UpdateValidatorAction?.Invoke();
        }
    }
}