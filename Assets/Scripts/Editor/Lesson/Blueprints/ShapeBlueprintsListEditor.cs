using System;
using System.Linq;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Lesson.Shapes.Datas;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson.Blueprints
{
    public class ShapeBlueprintsListEditor : IHaveVisualElement
    {
        private ShapeBlueprintFactory m_ShapeBlueprintFactory;
        
        private VisualElement m_RootVisualElement;

        private VisualElement m_OriginField;
        // Contains list of blueprint editors
        private VisualElement m_BaseVisualElement;
        // Contains 'Create' button
        private VisualElement m_BottomVisualElement; 
        
        public VisualElement GetVisualElement()
        {
            m_RootVisualElement = new Foldout {text = "Shapes Set"};
            UpdateCanvas();
            return m_RootVisualElement;
        }

        private void UpdateCanvas()
        {
            m_RootVisualElement.Clear();

            m_OriginField = GetOriginField();
            m_BaseVisualElement = GetBaseVisualElement();
            m_BottomVisualElement = GetBottomVisualElement();

            m_RootVisualElement.Add(m_OriginField);
            m_RootVisualElement.Add(m_BaseVisualElement);
            m_RootVisualElement.Add(m_BottomVisualElement);
        }

        private VisualElement GetOriginField()
        {
            if (m_ShapeBlueprintFactory == null)
            {
                return new VisualElement();
            }
            
            Vector3Field originField = new Vector3Field("Origin");

            originField.RegisterCallback<ChangeEvent<Vector3>>(evt =>
                m_ShapeBlueprintFactory.ShapeDataFactory.SetOrigin(evt.newValue));

            originField.value = m_ShapeBlueprintFactory.ShapeDataFactory.Center;
            
            return originField;
        }

        private VisualElement GetBaseVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            if (m_ShapeBlueprintFactory == null)
            {
                return visualElement;
            }
            
            foreach (ShapeBlueprint blueprint in m_ShapeBlueprintFactory.ShapeBlueprints)
            {
                visualElement.Add(ShapeBlueprintEditorFactory.GetVisualElement(blueprint, RemoveBlueprint));
            }

            return visualElement;
        }

        private VisualElement GetBottomVisualElement()
        {
            VisualElement visualElement = new VisualElement();
            
            if (m_ShapeBlueprintFactory == null)
            {
                return visualElement;
            }

            ToolbarMenu blueprintsList = new ToolbarMenu {text = "Create new Blueprint", style = { flexDirection = FlexDirection.Row}};
            blueprintsList.RemoveFromClassList("unity-toolbar-menu");
            blueprintsList.AddToClassList("unity-button");
            blueprintsList.AddToClassList("create");

            CreateDropdown(blueprintsList.menu);
            visualElement.Add(blueprintsList);
            
            return visualElement;
        }

        public void OnTargetChosen(ShapeBlueprintFactory target)
        {
            m_ShapeBlueprintFactory = target;

            UpdateCanvas();
        }

        private void CreateDropdown(DropdownMenu menu)
        {
            foreach (ShapeBlueprintFactory.ShapeBlueprintType shapeType in Enum
                .GetValues(typeof(ShapeBlueprintFactory.ShapeBlueprintType))
                .Cast<ShapeBlueprintFactory.ShapeBlueprintType>())
            {
                menu.AppendAction(
                    shapeType.ToString(), 
                    menuAction => CreateBlueprint(shapeType),
                    DropdownMenuAction.AlwaysEnabled);
            }
        }

        private void CreateBlueprint(ShapeBlueprintFactory.ShapeBlueprintType blueprintType)
        {
            ShapeBlueprint blueprint = m_ShapeBlueprintFactory.CreateShapeBlueprint(blueprintType);
            VisualElement visualElement = ShapeBlueprintEditorFactory.GetVisualElement(blueprint, RemoveBlueprint);
            m_BaseVisualElement.Add(visualElement);
        }

        private void RemoveBlueprint(ShapeBlueprint blueprint, VisualElement blueprintVisualElement)
        {
            m_ShapeBlueprintFactory.Remove(blueprint);
            m_BaseVisualElement.Remove(blueprintVisualElement);
        }
    }
}