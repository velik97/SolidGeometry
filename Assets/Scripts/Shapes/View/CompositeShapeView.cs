using System.Linq;
using Shapes.Data;

namespace Shapes.View
{
    public class CompositeShapeView : IShapeView
    {
        private readonly CompositeShapeData m_CompositeShapeData;

        public CompositeShapeView(CompositeShapeData compositeShapeData)
        {
            m_CompositeShapeData = compositeShapeData;
        }

        public void SetActive(bool value)
        {
            foreach (PointView point in m_CompositeShapeData.Points.Select(p => p.PointView))
            {
                point.SetActive(value);
            }
            foreach (LineView line in m_CompositeShapeData.Lines.Select(p => p.LineView))
            {
                line.SetActive(value);
            }
            foreach (PolygonView polygon in m_CompositeShapeData.Polygons.Select(p => p.PolygonView))
            {
                polygon.SetActive(value);
            }
        }

        public void SetHighlight(HighlightType highlightType)
        {
            foreach (PointView point in m_CompositeShapeData.Points.Select(p => p.PointView))
            {
                point.SetHighlight(highlightType);
            }
            foreach (LineView line in m_CompositeShapeData.Lines.Select(p => p.LineView))
            {
                line.SetHighlight(highlightType);
            }
            foreach (LineView polygon in m_CompositeShapeData.Lines.Select(p => p.LineView))
            {
                polygon.SetHighlight(highlightType);
            }
        }

        public void Release() { }
    }
}