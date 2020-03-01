using Editor.VisualElementsExtensions;
using LessonComponents;
using Stages;
using UnityEngine.UIElements;

namespace Editor.Lesson
{
    public class StagesSetEditor : IHaveVisualElement
    {
        public VisualElement GetVisualElement()
        {
            return new VisualElement();
        }

        public void OnTargetChosen(LessonStageFactory stagesSet)
        {
            
        }
    }
}