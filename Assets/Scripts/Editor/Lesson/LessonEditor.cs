using System;
using ConstructorVisualization;
using Editor.Lesson.Blueprints;
using Editor.Lesson.Stages;
using Editor.VisualElementsExtensions;
using Lesson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class LessonEditor : EditorWindow, ILessonDataCarrier
    {
        private Label m_LessonNameLabel;
        private VisualElement m_RootScroll;

        private LessonFileSaveLoadEditor m_LessonFileSaveLoadEditor;
        private ShapeBlueprintsListEditor m_ShapeBlueprintsListEditor;
        private LessonStagesListEditor m_LessonStagesListEditor;

        private LessonData m_LessonData;
        
        private string m_LessonName;
        private bool m_LessonIsDirty;

        [MenuItem("Tools/Lessons Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<LessonEditor>();
            
            window.titleContent = new GUIContent("Lessons Editor");
            window.minSize = new Vector2(250, 500);
        }

        private void OnEnable()
        {
            m_LessonFileSaveLoadEditor = new LessonFileSaveLoadEditor(this);
            m_ShapeBlueprintsListEditor = new ShapeBlueprintsListEditor();
            m_LessonStagesListEditor = new LessonStagesListEditor();
            
            m_RootScroll = new ScrollView();
            rootVisualElement.Clear();
            rootVisualElement.Add(m_LessonNameLabel = new Label());
            m_LessonNameLabel.AddToClassList("header");
            rootVisualElement.Add(m_RootScroll);

            VisualElement lessonSaveLoadEditorElement = m_LessonFileSaveLoadEditor.GetVisualElement();
            lessonSaveLoadEditorElement.AddToClassList("save-load-container");
            
            VisualElement lessonBlueprintsEditorElement = m_ShapeBlueprintsListEditor.GetVisualElement();
            lessonBlueprintsEditorElement.AddToClassList("blueprints-container");
            
            VisualElement lessonStagesEditorElement = m_LessonStagesListEditor.GetVisualElement();
            lessonStagesEditorElement.AddToClassList("stages-container");
            
            m_RootScroll.Add(lessonSaveLoadEditorElement);
            m_RootScroll.Add(lessonBlueprintsEditorElement);
            m_RootScroll.Add(lessonStagesEditorElement);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Styles/lesson_editor_styles.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
            
            rootVisualElement.Add(new VisualElementUpdateInInvoker());
        }
        
        public LessonData GetLessonData()
        {
            return m_LessonData;
        }

        public void SetLessonData(LessonData lessonData, string lessonName)
        {
            if (m_LessonData != null)
            {
                m_LessonData.DirtinessChanged -= UpdateLessonName;
            }
            m_LessonData = lessonData;
            m_LessonData.DirtinessChanged += UpdateLessonName;

            m_LessonName = lessonName;
            m_LessonIsDirty = !m_LessonData.IsDirty;
            UpdateLessonName();

            m_ShapeBlueprintsListEditor.OnTargetChosen(m_LessonData.ShapeBlueprintFactory);
            m_LessonStagesListEditor.OnTargetChosen(m_LessonData.LessonStageFactory);

            LessonVisualizer lessonVisualizer = FindObjectOfType<LessonVisualizer>();
            if (lessonVisualizer != null)
            {
                lessonVisualizer.SetShapeDataFactory(m_LessonData.ShapeDataFactory);
            }
        }

        public void CreateNewLesson(string lessonName)
        {
            SetLessonData(new LessonData(), lessonName);
        }

        private void UpdateLessonName()
        {
            if (m_LessonData.IsDirty == m_LessonIsDirty)
            {
                return;
            }

            m_LessonIsDirty = m_LessonData.IsDirty;

            m_LessonNameLabel.text = m_LessonName + (m_LessonIsDirty ? "*" : "");
        }

        private void OnGUI()
        {
            Handles.SphereHandleCap(0, Vector3.one, Quaternion.identity, 1f, EventType.Ignore);
        }
    }
}