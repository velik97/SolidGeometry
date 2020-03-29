using System.Linq;
using Lesson.Shapes.Datas;

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
    }
}