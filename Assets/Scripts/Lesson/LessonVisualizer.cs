using System;
using Shapes.View;
using UnityEngine;

namespace Lesson
{
    [RequireComponent(typeof(ShapeViewFactory))]
    public class LessonVisualizer : MonoBehaviour
    {
        [SerializeField] private LessonBlueprint m_LessonBlueprint;

        private ShapeViewFactory m_ShapeViewFactory;

        private ShapeViewFactory ShapeViewFactory =>
            m_ShapeViewFactory ?? (m_ShapeViewFactory = GetComponent<ShapeViewFactory>());

        private LessonBlueprint m_PreviousLessonBlueprint;

        private void OnValidate()
        {
            if (m_LessonBlueprint != m_PreviousLessonBlueprint)
            {
                m_LessonBlueprint?.ShapeDataFactory.SetViewFactory(ShapeViewFactory);
            }

            m_PreviousLessonBlueprint = m_LessonBlueprint;
        }
    }
}