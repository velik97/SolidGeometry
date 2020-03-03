using Lesson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class LessonEditor : EditorWindow
    {
        private VisualElement m_RootVisualElement;

        private LessonFilesListEditor m_LessonFilesListEditor;
        private ShapeBlueprintListEditor m_ShapeBlueprintListEditor;
        private StageListEditor m_StageListEditor;

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
            m_LessonFilesListEditor = new LessonFilesListEditor(GetLessonData, SetLessonData, CreateNewLesson);
            m_ShapeBlueprintListEditor = new ShapeBlueprintListEditor();
            m_StageListEditor = new StageListEditor();
            
            m_RootVisualElement = new ScrollView();
            rootVisualElement.Clear();
            rootVisualElement.Add(m_RootVisualElement);
            
            m_RootVisualElement.Add(m_LessonFilesListEditor.GetVisualElement());
            m_RootVisualElement.Add(m_ShapeBlueprintListEditor.GetVisualElement());
            m_RootVisualElement.Add(m_StageListEditor.GetVisualElement());
        }

        private LessonData GetLessonData()
        {
            return m_LessonData;
        }

        private void SetLessonData(LessonData lessonData)
        {
            m_LessonData = lessonData;

            m_ShapeBlueprintListEditor.OnTargetChosen(m_LessonData.ShapeBlueprintFactory);
            m_StageListEditor.OnTargetChosen(m_LessonData.LessonStageFactory);

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