namespace Shapes.View
{
    public class CompositeShapeView : IShapeView
    {
        private PointView[] m_Points;
        private LineView[] m_Lines;
        private PolygonView[] m_Polygons;

        public CompositeShapeView(PointView[] points, LineView[] lines, PolygonView[] polygons)
        {
            m_Points = points;
            m_Lines = lines;
            m_Polygons = polygons;
        }

        public void SetActive(bool value)
        {
            foreach (PointView point in m_Points)
            {
                point.SetActive(value);
            }
            foreach (LineView line in m_Lines)
            {
                line.SetActive(value);
            }
            foreach (var polygon in m_Polygons)
            {
                polygon.SetActive(value);
            }
        }

        public void SetHighlight(HighlightType highlightType)
        {
            foreach (PointView point in m_Points)
            {
                point.SetHighlight(highlightType);
            }
            foreach (LineView line in m_Lines)
            {
                line.SetHighlight(highlightType);
            }
            foreach (var polygon in m_Polygons)
            {
                polygon.SetHighlight(highlightType);
            }
        }
    }
}