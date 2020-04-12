using System.Linq;
using Lesson.Shapes.Datas;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lesson.Shapes.Views
{
    public class CompositeShapeView : IShapeView
    {
        private readonly CompositeShapeData m_CompositeShapeData;

        public CompositeShapeView(CompositeShapeData compositeShapeData)
        {
            m_CompositeShapeData = compositeShapeData;
        }

        private bool m_Active = false;
        private HighlightType m_Highlight = HighlightType.Normal;

        public bool Active
        {
            get => m_Active;
            set
            {
                m_Active = value;
                SetActive(value);
            }
        }
        
        public HighlightType Highlight 
        {
            get => m_Highlight;
            set
            {
                m_Highlight = value;
                SetHighlight(value);
            }
        }

        private void SetActive(bool value)
        {
            foreach (PointView point in m_CompositeShapeData.Points.Select(p => p.PointView))
            {
                point.Active = value;
            }
            foreach (LineView line in m_CompositeShapeData.Lines.Select(p => p.LineView))
            {
                line.Active = value;
            }
            foreach (PolygonView polygon in m_CompositeShapeData.Polygons.Select(p => p.PolygonView))
            {
                polygon.Active = value;
            }
        }

        private void SetHighlight(HighlightType value)
        {
            foreach (PointView point in m_CompositeShapeData.Points.Select(p => p.PointView))
            {
                point.Highlight = value;
            }
            foreach (LineView line in m_CompositeShapeData.Lines.Select(p => p.LineView))
            {
                line.Highlight = value;
            }
            foreach (PolygonView polygon in m_CompositeShapeData.Polygons.Select(p => p.PolygonView))
            {
                polygon.Highlight = value;
            }
        }

        public void SelectInEditor()
        {
            Object[] gameObjects = new Object[m_CompositeShapeData.Points.Length +
                                                      m_CompositeShapeData.Lines.Length +
                                                      m_CompositeShapeData.Polygons.Length];
            int i = 0;
            
            foreach (PointView point in m_CompositeShapeData.Points.Select(p => p.PointView))
            {
                gameObjects[i] = point.gameObject;
                i++;
            }
            foreach (LineView line in m_CompositeShapeData.Lines.Select(p => p.LineView))
            {
                gameObjects[i] = line.gameObject;
                i++;
            }
            foreach (PolygonView polygon in m_CompositeShapeData.Polygons.Select(p => p.PolygonView))
            {
                gameObjects[i] = polygon.gameObject;
                i++;
            }
            
#if UNITY_EDITOR
            Selection.objects = gameObjects;
#endif
        }
    }
}