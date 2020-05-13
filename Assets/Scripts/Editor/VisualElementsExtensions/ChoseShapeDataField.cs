using System;
using JetBrains.Annotations;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Datas;
using Lesson.Validators;
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

        private IVisualElementScheduledItem m_AutoSaveScheduler;
        private bool m_NeedUpdate = false;

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
            
            m_ToolbarMenu = new ToolbarMenu {text = "Change", style = { flexDirection = FlexDirection.Row}};
            m_ToolbarMenu.RemoveFromClassList("unity-toolbar-menu");
            m_ToolbarMenu.AddToClassList("unity-button");
            
            firstRow.Add(m_ToolbarMenu);
            
            Add(firstRow);

            IValidator pointNotEmptyValidator =
                new DataNotEmptyValidator<TShapeData>(getShapeDataFunc, action => m_UpdateValidatorAction = action);
            VisualElement validatorField = new ValidatorField(pointNotEmptyValidator);
            Add(validatorField);

            m_DataFactory.BecameDirty += BecameDirty;
            m_AutoSaveScheduler = schedule.Execute(Update);
            m_AutoSaveScheduler.Every(200);

            UpdateList();
            UpdateName();
            m_UpdateValidatorAction?.Invoke();
        }

        private void UpdateName()
        {
            m_Label.text = m_FieldName + " " + m_GetShapeDataFunc()?.ToString();;
        }

        private void BecameDirty()
        {
            m_NeedUpdate = true;
        }

        private void Update()
        {
            if (!m_NeedUpdate)
            {
                return;
            }
            m_NeedUpdate = false;
            UpdateList();
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