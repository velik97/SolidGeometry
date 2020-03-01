using Lesson;
using LessonComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class LessonEditor : EditorWindow
    {
        private VisualElement m_RootVisualElement;

        private LessonsListEditor m_LessonsListEditor;
        private ShapeBlueprintsEditor m_ShapeBlueprintsEditor;
        private StagesSetEditor m_StagesSetEditor;

        private LessonData m_LessonData;

        [MenuItem("Tools/Lessons Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<LessonEditor>();
            
            window.titleContent = new GUIContent("Lessons Editor");
            window.minSize = new Vector2(250, 500);
        }

        private void OnEnable()
        {
            m_LessonsListEditor = new LessonsListEditor(GetLessonData, SetLessonData, CreateNewLesson);
            m_ShapeBlueprintsEditor = new ShapeBlueprintsEditor();
            m_StagesSetEditor = new StagesSetEditor();
            
            m_RootVisualElement = new ScrollView();
            rootVisualElement.Clear();
            rootVisualElement.Add(m_RootVisualElement);
            
            m_RootVisualElement.Add(m_LessonsListEditor.GetVisualElement());
            m_RootVisualElement.Add(m_ShapeBlueprintsEditor.GetVisualElement());
            m_RootVisualElement.Add(m_StagesSetEditor.GetVisualElement());
        }

        private LessonData GetLessonData()
        {
            return m_LessonData;
        }

        private void SetLessonData(LessonData lessonData)
        {
            m_LessonData = lessonData;

            m_ShapeBlueprintsEditor.OnTargetChosen(m_LessonData.ShapeBlueprintFactory);
            m_StagesSetEditor.OnTargetChosen(m_LessonData.LessonStageFactory);

            LessonVisualizer lessonVisualizer = FindObjectOfType<LessonVisualizer>();
            if (lessonVisualizer != null)
            {
                lessonVisualizer.SetShapeDataFactory(m_LessonData.ShapeDataFactory);
            }
        }

        private void CreateNewLesson()
        {
            SetLessonData(new LessonData());
        }
    }
}