using Editor.VisualElementsExtensions;
using Lesson.Stages;
using UnityEngine.UIElements;

namespace Editor.Lesson.Stages
{
    public class LessonStagesListEditor : IHaveVisualElement
    {
        private LessonStageFactory m_LessonStageFactory;
        
        private VisualElement m_RootVisualElement;
        
        // Contains list of stage editors
        private VisualElement m_BaseVisualElement;
        // Contains 'Create' button
        private VisualElement m_BottomVisualElement; 
        
        public VisualElement GetVisualElement()
        {
            m_RootVisualElement = new Foldout {text = "Stages Set"};
            UpdateCanvas();
            return m_RootVisualElement;
        }

        public void OnTargetChosen(LessonStageFactory stagesFactory)
        {
            m_LessonStageFactory = stagesFactory;
            UpdateCanvas();
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

            if (m_LessonStageFactory == null)
            {
                return visualElement;
            }
            
            foreach (LessonStage lessonStage in m_LessonStageFactory.LessonStages)
            {
                visualElement.Add(new LessonStageEditor(lessonStage, RemoveStage, SwapStage).GetVisualElement());
            }

            return visualElement;
        }

        private VisualElement GetBottomVisualElement()
        {
            VisualElement visualElement = new VisualElement();
            
            if (m_LessonStageFactory == null)
            {
                return visualElement;
            }

            Button createButton = new Button(CreateStage) {text = "Create new Stage"};
            createButton.AddToClassList("create");

            visualElement.Add(createButton);
            
            return visualElement;
        }

        private void CreateStage()
        {
            LessonStage lessonStage = m_LessonStageFactory.CreateLessonStage();
            VisualElement visualElement = new LessonStageEditor(lessonStage, RemoveStage, SwapStage).GetVisualElement();
            m_BaseVisualElement.Add(visualElement);
        }

        private void RemoveStage(LessonStage stage, VisualElement blueprintVisualElement)
        {
            m_LessonStageFactory.Remove(stage);
            m_BaseVisualElement.Remove(blueprintVisualElement);
        }

        private void SwapStage(LessonStage stage, VisualElement element, bool up)
        {
            if (!m_BaseVisualElement.Contains(element))
            {
                return;
            }

            int elementIndex = m_BaseVisualElement.IndexOf(element);
            int swapElementIndex = elementIndex + (up ? -1 : 1);

            if (swapElementIndex < 0 || swapElementIndex >= m_BaseVisualElement.childCount)
            {
                return;
            }

            VisualElement swapElement = m_BaseVisualElement.ElementAt(swapElementIndex);

            if (swapElement.GetType() != element.GetType())
            {
                return;
            }

            if (!m_BaseVisualElement.SwapElementsAt(elementIndex, swapElementIndex))
            {
                return;
            }

            m_LessonStageFactory.SwapStages(stage, up);
        }
    }
}