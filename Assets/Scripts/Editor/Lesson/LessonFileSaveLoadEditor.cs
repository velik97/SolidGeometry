using System;
using System.IO;
using System.Linq;
using Editor.VisualElementsExtensions;
using Lesson;
using Newtonsoft.Json;
using Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class LessonFileSaveLoadEditor : IHaveVisualElement
    {
        private ILessonDataCarrier m_LessonDataCarrier;

        private string m_CurrentLessonPath;
        private string m_CurrentLessonName;

        private string CurrentLessonPath
        {
            get => m_CurrentLessonPath;
            set
            {
                m_CurrentLessonPath = value;
                m_CurrentLessonName = m_CurrentLessonPath
                    .Split('/')
                    .Last()
                    .Split('.')
                    .First();
            }
        }
        
        private static string FolderPath => Path.Combine(Application.dataPath, StaticPaths.FILES_FOLDER);
        private JsonSerializerSettings m_SerializerSettings = new JsonSerializerSettings
            {TypeNameHandling = TypeNameHandling.Auto};

        private IVisualElementScheduledItem m_AutoSaveScheduler;
        private Toggle m_AutoSaveToggle;

        public LessonFileSaveLoadEditor(ILessonDataCarrier lessonDataCarrier)
        {
            m_LessonDataCarrier = lessonDataCarrier;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();
            
            Button createNewButton = new Button(CreateNewFile) {text = "Create New"};
            visualElement.Add(createNewButton);
            
            Button choseFileButton = new Button(ChooseFile) {text = "Choose File"};
            visualElement.Add(choseFileButton);
            
            VisualElement saveElement = new VisualElement {style = { flexDirection = FlexDirection.Row}};

            m_AutoSaveToggle = new Toggle("Auto Save");
            saveElement.Add(m_AutoSaveToggle);
            
            Button saveButton = new Button(SaveRoutine) {text = "Save", style = { alignSelf = Align.Stretch}};
            saveElement.Add(saveButton);
            
            visualElement.Add(saveElement);

            m_AutoSaveScheduler = visualElement.schedule.Execute(AutoSaveRoutine);
            m_AutoSaveScheduler.Every(5000);
            m_AutoSaveScheduler.Pause();

            return visualElement;
        }

        private void AutoSaveRoutine()
        {
            if (!m_AutoSaveToggle.value)
            {
                return;
            }
            
            SaveRoutine();
        }

        private void SaveRoutine()
        {
            LessonData lessonData = m_LessonDataCarrier.GetLessonData();

            if (lessonData == null || CurrentLessonPath == null)
            {
                m_AutoSaveScheduler.Pause();
                return;
            }

            bool isDirty = lessonData.IsDirty;

            if (!isDirty)
            {
                return;
            }
            
            bool saved = TrySaveLesson();
            if (!saved)
            {
                m_AutoSaveScheduler.Pause();
            }
            else
            {
                lessonData.Saved();
            }
        }

        private void CreateNewFile()
        {
            string previousPath = CurrentLessonPath;
            CurrentLessonPath = EditorUtility.SaveFilePanel(
                "Create new lesson",
                FolderPath,
                "lesson",
                "json");

            if (string.IsNullOrEmpty(CurrentLessonPath))
            {
                CurrentLessonPath = previousPath;
                return;
            }
            
            m_LessonDataCarrier.CreateNewLesson(m_CurrentLessonName);

            TrySaveLesson("Can't create new file");
            m_AutoSaveScheduler.Resume();
        }

        private void ChooseFile()
        {
            string previousPath = CurrentLessonPath;
            CurrentLessonPath = EditorUtility.OpenFilePanel(
                "Choose lesson",
                FolderPath,
                "json");
            
            if (string.IsNullOrEmpty(CurrentLessonPath))
            {
                CurrentLessonPath = previousPath;
                return;
            }

            LessonData deserializedLesson;
            
            try
            {
                string json = File.ReadAllText(CurrentLessonPath);
                deserializedLesson = JsonConvert.DeserializeObject<LessonData>(json, m_SerializerSettings);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while deserializing: {e}");
                throw;
            }

            m_LessonDataCarrier.SetLessonData(deserializedLesson, m_CurrentLessonName);
            m_AutoSaveScheduler.Resume();
        }

        private bool TrySaveLesson(string errorMessage = "Can't save lesson")
        {
            try
            {
                SaveLesson();
            }
            catch (Exception e)
            {
                Debug.LogError($"{errorMessage}: {e}'");
                return false;
            }
            return true;
        }

        private void SaveLesson()
        {
            if (CurrentLessonPath == null)
            {
                Debug.LogError("Path for saving is empty");
                throw new NullReferenceException();
            }

            LessonData lessonData = m_LessonDataCarrier.GetLessonData();

            if (lessonData == null)
            {
                Debug.LogError("LessonData for saving is empty");
                throw new NullReferenceException();
            }

            string serializedLesson;
            
            try
            {
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
                serializedLesson = JsonConvert.SerializeObject(lessonData, Formatting.Indented, serializerSettings);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while serializing: {e}");
                Console.WriteLine(e);
                throw;
            }

            try
            {
                File.WriteAllText(CurrentLessonPath, serializedLesson);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while writing file: {e}");
                throw;
            }
        }
    }
}