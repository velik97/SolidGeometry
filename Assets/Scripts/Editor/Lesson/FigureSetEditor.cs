using System;
using System.IO;
using System.Linq;
using Editor.Blueprints;
using Editor.VisualElementsExtensions;
using Lesson;
using Serialization;
using Shapes.Blueprint;
using Shapes.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class FigureSetEditor : EditorWindow
    {
        private FiguresSet m_Target;
        
        private VisualElement m_RootVisualElement;
        
        private VisualElement m_TopVisualElement;
        private VisualElement m_BaseVisualElement;
        private VisualElement m_BottomVisualElement;

        private Foldout m_DebugElement;

        private string FolderName =>
            Path.Combine(Application.dataPath, StaticPaths.FILES_FOLDER, StaticPaths.FIGURE_SETS_FOLDER);
        
        FolderJsonsListSerializer<FiguresSet> m_Serializer;


        [MenuItem("Tools/Figure Set Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<FigureSetEditor>();
            
            window.titleContent = new GUIContent("Figure Set Editor");
            window.minSize = new Vector2(250, 500);
        }

        private void OnEnable()
        {
            m_Serializer = new FolderJsonsListSerializer<FiguresSet>(FolderName);
            m_RootVisualElement = new ScrollView();
            rootVisualElement.Add(m_RootVisualElement);
            
            m_RootVisualElement.Clear();
            m_TopVisualElement = GetTopVisualElement();
            m_RootVisualElement.Add(m_TopVisualElement);

            m_Serializer.UpdateList();
            if (m_Serializer.Names().Count == 0)
            {
                OnTargetChosen(new FiguresSet());
            }
        }

        private void UpdateCanvas()
        {
            m_RootVisualElement.Clear();
            
            m_BaseVisualElement = GetBaseVisualElement();
            m_BottomVisualElement = GetBottomVisualElement();
            
            m_RootVisualElement.Add(m_TopVisualElement);
            m_RootVisualElement.Add(m_BaseVisualElement);
            m_RootVisualElement.Add(m_BottomVisualElement);
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

        private VisualElement GetTopVisualElement()
        {
            VisualElement visualElement = new VisualElement();
            
            VisualElement serializerFoldout = new Foldout {text = "All Files"};
            serializerFoldout.Add(new ListOfFilesInFolder<FiguresSet>(m_Serializer, OnTargetChosen));

            TextField saveName = new TextField("Save Name");
            Button saveButton = new Button(() => SaveCurrentFigureSet(saveName.value)) {text = "Save"};
            serializerFoldout.Add(saveName);
            serializerFoldout.Add(saveButton);
            
            visualElement.Add(serializerFoldout);
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
            UpdateDebug();
            return visualElement;
        }

        private void SaveCurrentFigureSet(string fileName)
        {
            m_Serializer.SaveObject(m_Target, fileName);
        }

        private void OnTargetChosen(FiguresSet target)
        {
            m_Target?.Clear();
            m_Target = target;
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