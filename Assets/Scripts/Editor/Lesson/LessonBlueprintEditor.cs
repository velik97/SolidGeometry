using System;
using System.Linq;
using Editor.Shapes;
using Shapes.Blueprint;
using Shapes.Data;
using Shapes.Lesson;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    [CustomEditor(typeof(LessonBlueprint))]
    public class LessonBlueprintEditor : UnityEditor.Editor
    {
        private LessonBlueprint m_Target;
        
        private VisualElement m_RootVisualElement;
        
        private VisualElement m_TopVisualElement;
        private VisualElement m_BaseVisualElement;
        private VisualElement m_BottomVisualElement;

        private Foldout m_DebugElement;

        private void OnEnable()
        {
            m_Target = target as LessonBlueprint;
            m_RootVisualElement = new VisualElement();
        }

        public override VisualElement CreateInspectorGUI()
        {
            m_RootVisualElement.Clear();
            
            m_TopVisualElement = new VisualElement();
            m_BaseVisualElement = GetBaseVisualElement();
            m_BottomVisualElement = GetBottomVisualElement();
            
            m_RootVisualElement.Add(m_TopVisualElement);
            m_RootVisualElement.Add(m_BaseVisualElement);
            m_RootVisualElement.Add(m_BottomVisualElement);

            return m_RootVisualElement;
        }

        private VisualElement GetBaseVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            foreach (ShapeBlueprint blueprint in m_Target.ShapeBlueprints)
            {
                visualElement.Add(ShapeBlueprintEditorsFactory.GetVisualElement(blueprint, RemoveBlueprint));
            }

            return visualElement;
        }

        private VisualElement GetBottomVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            ToolbarMenu blueprintsList = new ToolbarMenu {text = "Create"};
            CreateDropdown(blueprintsList.menu);
            visualElement.Add(blueprintsList);
            
            m_DebugElement = new Foldout {text = "All Datas"};
            m_Target.ShapeDataFactory.ShapesListUpdated += UpdateDebug;
            visualElement.Add(m_DebugElement);
            return visualElement;
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
            foreach (ShapeData shapeData in m_Target.ShapeDataFactory.AllDatas)
            {
                m_DebugElement.Insert(i++, new Label(shapeData.ToString()));
            }
        }

        private void CreateBlueprint(ShapeBlueprintFactory.ShapeBlueprintType blueprintType)
        {
            ShapeBlueprint blueprint = m_Target.AddBlueprint(blueprintType);
            VisualElement visualElement = ShapeBlueprintEditorsFactory.GetVisualElement(blueprint, RemoveBlueprint);
            m_BaseVisualElement.Add(visualElement);
        }

        private void RemoveBlueprint(ShapeBlueprint blueprint, VisualElement blueprintVisualElement)
        {
            m_Target.DeleteBlueprint(blueprint);
            m_BaseVisualElement.Remove(blueprintVisualElement);
        }
    }
}