using System;
using System.Linq;
using Shapes.View;

namespace Shapes.Data
{
    [Serializable]
    public class CompositeShapeData : ShapeData
    {
        CompositeShapeView CompositeShapeView => View as CompositeShapeView;
        
        private PointData[] m_Points;
        private LineData[] m_Lines;
        private PolygonData[] m_Polygons;

        public PointData[] Points => m_Points;
        public LineData[] Lines => m_Lines;
        public PolygonData[] Polygons => m_Polygons;

        private string m_ShapeName;

        public CompositeShapeData()
        {
        }

        public void SetShapeName(string shapeName)
        {
            m_ShapeName = shapeName;
            OnNameUpdated();
        }

        public void SetPoints(PointData[] points)
        {
            m_Points = points;
            OnNameUpdated();
        }

        public void SetLines(LineData[] lines)
        {
            m_Lines = lines;
        }

        public void SetPolygons(PolygonData[] polygons)
        {
            m_Polygons = polygons;
        }

        public override string ToString()
        {
            return (m_ShapeName ?? "") + " " +
                   (m_Points != null ? string.Join("", m_Points.Select(p => p?.PointName)) : "");
        }
    }
}