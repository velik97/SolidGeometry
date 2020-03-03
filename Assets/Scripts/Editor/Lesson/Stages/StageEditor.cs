using System;
using Editor.VisualElementsExtensions;
using Lesson.Stages;
using UnityEngine.UIElements;

namespace Editor.Lesson.Stages
{
    public class StageEditor : IHaveVisualElement
    {
        private readonly LessonStage m_Stage;
        private readonly Action<LessonStage, VisualElement> m_DeleteAction;

        private Foldout m_NameElement;
        private Button m_DeleteButton;

        public StageEditor(LessonStage stage, Action<LessonStage, VisualElement> deleteAction)
        {
            m_Stage = stage;
            m_DeleteAction = deleteAction;

            m_Stage.NameUpdated += UpdateName;
            m_Stage.NumUpdated += UpdateName;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            m_NameElement = new Foldout();
            m_NameElement.Insert(0, visualElement);
            
            TextField nameField = new TextField("Name: ") {value = m_Stage.StageName};
            TextField descriptionField = new TextField("Description: ") {value = m_Stage.StageDescription};
            
            nameField.RegisterCallback<ChangeEvent<string>>(evt => m_Stage.SetName(evt.newValue));
            descriptionField.RegisterCallback<ChangeEvent<string>>(evt => m_Stage.SetDescription(evt.newValue));
            
            visualElement.Add(nameField);
            visualElement.Add(descriptionField);

            m_DeleteButton = new Button(() => m_DeleteAction(m_Stage, m_NameElement)) {text = "Delete"};
            visualElement.Add(m_DeleteButton);

            UpdateName();
            return m_NameElement;
        }

        private void UpdateName()
        {
            m_NameElement.text = $"{m_Stage.StageNum + 1} {m_Stage.StageName}";
        }
    }
}