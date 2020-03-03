using System;
using System.IO;
using Editor.VisualElementsExtensions;
using Lesson;
using Serialization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class LessonFilesListEditor : IHaveVisualElement
    {
        private static string FolderName =>
            Path.Combine(Application.dataPath, StaticPaths.FILES_FOLDER);
        
        private FolderJsonsListSerializer<LessonData> m_Serializer;

        private Func<LessonData> m_GetLessonFunc;
        private Action<LessonData> m_LessonChosenAction;
        private Action m_CreateNewAction;

        public LessonFilesListEditor(Func<LessonData> getLessonFunc, Action<LessonData> lessonChosenAction, Action createNewAction)
        {
            m_GetLessonFunc = getLessonFunc;
            m_LessonChosenAction = lessonChosenAction;
            m_CreateNewAction = createNewAction;
            
            m_Serializer = new FolderJsonsListSerializer<LessonData>(FolderName);
        }

        public VisualElement GetVisualElement()
        {
            m_Serializer.UpdateList();
            
            VisualElement foldout = new Foldout {text = "All Files"};
            foldout.Add(new ListOfFilesInFolder<LessonData>(m_Serializer, m_LessonChosenAction));
            
            Button createNewButton = new Button(m_CreateNewAction) {text = "Create Empty"};
            foldout.Add(createNewButton);
            
            Button updateButton = new Button(m_Serializer.UpdateList) {text = "Update"};
            foldout.Add(updateButton);

            TextField saveName = new TextField("Save Name");
            Button saveButton = new Button(() => SaveLesson(saveName.value)) {text = "Save"};
            foldout.Add(saveName);
            foldout.Add(saveButton);

            return foldout;
        }
        
        private void SaveLesson(string fileName)
        {
            m_Serializer.SaveObject(m_GetLessonFunc(), fileName);
        }
    }
}