using System;
using System.Linq;
using Editor.Lesson.Blueprints;
using Editor.VisualElementsExtensions;
using Lesson.Shapes.Blueprints;
using Shapes.Blueprint;
using Shapes.Data;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class ShapeBlueprintsListEditor : IHaveVisualElement
    {
        private ShapeBlueprintFactory m_ShapeBlueprintFactory;
        
        private VisualElement m_RootVisualElement;
        
        // Contains list of blueprint editors
        private VisualElement m_BaseVisualElement;
        // Contains 'Create' button and debug element with all shape datas
        private VisualElement m_BottomVisualElement; 

        private Foldout m_DebugElement;

        public VisualElement GetVisualElement()
        {
            m_RootVisualElement = new Foldout {text = "Shapes Set"};
            UpdateCanvas();
            return m_RootVisualElement;
        }

        private void UpdateCanvas()
        {
            m_RootVisualElement.Clear();
            
            m_BaseVisualElement = GetBaseVisualElement();
            m_BottomVisualElement = GetBottomVisualElement();
            
            m_RootVisualElement.Add(m_BaseVisualElement);
            m_RootVisualElement.Add(m_BottomVisualElement);
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

            ToolbarMenu blueprintsList = new ToolbarMenu {text = "Create"};
            CreateDropdown(blueprintsList.menu);
            visualElement.Add(blueprintsList);
            
            m_DebugElement = new Foldout {text = "All Datas"};
            m_ShapeBlueprintFactory.ShapeDataFactory.ShapesListUpdated += UpdateDebug;
            visualElement.Add(m_DebugElement);
            UpdateDebug();
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

        private void UpdateDebug()
        {
            m_DebugElement.Clear();
            int i = 0;
            foreach (ShapeData shapeData in m_ShapeBlueprintFactory.ShapeDataFactory.AllDatas)
            {
                m_DebugElement.Insert(i++, new Label(shapeData.ToString()));
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