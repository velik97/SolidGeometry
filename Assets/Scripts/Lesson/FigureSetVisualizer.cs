using System;
using Shapes.Data;
using Shapes.View;
using UnityEngine;

namespace Lesson
{
    [RequireComponent(typeof(ShapeViewFactory))]
    public class FigureSetVisualizer : MonoBehaviour
    {
        private ShapeViewFactory m_ShapeViewFactory;

        private ShapeViewFactory ShapeViewFactory =>
            m_ShapeViewFactory ?? (m_ShapeViewFactory = GetComponent<ShapeViewFactory>());
        
        public void SetShapeDataFactory(ShapeDataFactory shapeDataFactory)
        {
            ShapeViewFactory.Clear();
            shapeDataFactory.SetViewFactory(ShapeViewFactory);
        }
    }
}