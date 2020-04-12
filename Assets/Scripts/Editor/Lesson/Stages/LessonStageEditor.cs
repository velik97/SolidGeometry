using System;
using System.Linq;
using Editor.Lesson.Stages.Actions;
using Editor.VisualElementsExtensions;
using Lesson.Stages;
using Lesson.Stages.Actions;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Lesson.Stages
{
    public class LessonStageEditor : IHaveVisualElement
    {
        private readonly LessonStage m_Stage;
        private readonly Action<LessonStage, VisualElement> m_DeleteAction;

        private Foldout m_NameElement;
        
        // Contains list of shape action editors
        private VisualElement m_ShapeActionsListVisualElement;
        // Contains 'Create' button
        private VisualElement m_CreateShapeActionVisualElement; 

        public LessonStageEditor(LessonStage stage, Action<LessonStage, VisualElement> deleteAction)
        {
            m_Stage = stage;
            m_DeleteAction = deleteAction;

            m_Stage.BecameDirty += UpdateName;
            m_Stage.BecameDirty += UpdateName;
        }

        public VisualElement GetVisualElement()
        {
            VisualElement visualElement = new VisualElement();

            m_NameElement = new Foldout();
            m_NameElement.Insert(0, visualElement);

            visualElement.Add(GetNameFieldVisualElement());
            visualElement.Add(GeDescriptionFieldVisualElement());

            VisualElement actionsElement = new Foldout {text = "Actions"};
            actionsElement.Add(GetShapeActionsListVisualElement());
            actionsElement.Add(new ValidatorField(m_Stage.NoConflictsBetweenShapeActionsValidator));
            actionsElement.Add(GetCreateShapeActionVisualElement());
            visualElement.Add(actionsElement);
            
            visualElement.Add(GetDeleteStageVisualElement());
            
            m_NameElement.AddToClassList("container");

            UpdateName();
            return m_NameElement;
        }

        private VisualElement GetNameFieldVisualElement()
        {
            TextField nameField = new TextField("Name: ") {value = m_Stage.StageName};
            nameField.RegisterCallback<ChangeEvent<string>>(evt => m_Stage.SetName(evt.newValue));
            return nameField;
        }
        
        private VisualElement GeDescriptionFieldVisualElement()
        {
            TextField descriptionField = new TextField("Description: ")
            {
                value = m_Stage.StageDescription,
                multiline = true,
                style = { whiteSpace = WhiteSpace.Normal}
            };
            descriptionField.RegisterCallback<ChangeEvent<string>>(evt => m_Stage.SetDescription(evt.newValue));
            return descriptionField;
        }

        private VisualElement GetShapeActionsListVisualElement()
        {
            m_ShapeActionsListVisualElement = new VisualElement();
            foreach (ShapeAction shapeAction in m_Stage.ShapeActions)
            {
                VisualElement visualElement = ShapeActionEditorFactory.GetVisualElement(shapeAction, RemoveShapeAction);
                m_ShapeActionsListVisualElement.Add(visualElement);
            }
            return m_ShapeActionsListVisualElement;
        }

        private VisualElement GetCreateShapeActionVisualElement()
        {
            ToolbarMenu shapeActionTypesList = new ToolbarMenu {text = "Add Action", style = { flexDirection = FlexDirection.Row}};
            shapeActionTypesList.RemoveFromClassList("unity-toolbar-menu");
            shapeActionTypesList.AddToClassList("unity-button");
            shapeActionTypesList.AddToClassList("create");

            foreach (ShapeActionFactory.ShapeActionType actionType in Enum
                .GetValues(typeof(ShapeActionFactory.ShapeActionType))
                .Cast<ShapeActionFactory.ShapeActionType>())
            {
                shapeActionTypesList.menu.AppendAction(
                    actionType.ToString(),
                    menuAction => AddShapeAction(actionType),
                    DropdownMenuAction.AlwaysEnabled);
            }

            return shapeActionTypesList;
        }

        private void AddShapeAction(ShapeActionFactory.ShapeActionType actionType)
        {
            ShapeAction shapeAction = m_Stage.AddAction(actionType);
            VisualElement visualElement = ShapeActionEditorFactory.GetVisualElement(shapeAction, RemoveShapeAction);
            m_ShapeActionsListVisualElement.Add(visualElement);
        }
        
        private void RemoveShapeAction(ShapeAction shapeAction, VisualElement visualElement)
        {
            m_Stage.RemoveAction(shapeAction);
            m_ShapeActionsListVisualElement.Remove(visualElement);
        }

        private VisualElement GetDeleteStageVisualElement()
        {
            Button deleteButton = new Button(() => m_DeleteAction(m_Stage, m_NameElement)) {text = "Delete"};
            deleteButton.AddToClassList("delete");

            return deleteButton;
        }

        private void UpdateName()
        {
            m_NameElement.text = $"{m_Stage.StageNum + 1} {m_Stage.StageName}";
        }
    }
}