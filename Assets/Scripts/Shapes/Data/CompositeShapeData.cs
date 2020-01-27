using System;
using System.Linq;
using Shapes.View;

namespace Shapes.Data
{
    [Serializable]
    public class CompositeShapeData : ShapeData
    {
        CompositeShapeView CompositeShapeView => View as CompositeShapeView;
        
        public PointData[] Points;
        public LineData[] Lines;
        public PolygonData[] Polygons;

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
            Points = points;
            OnNameUpdated();
        }

        public void SetLines(LineData[] lines)
        {
            Lines = lines;
        }

        public void SetPolygons(PolygonData[] polygons)
        {
            Polygons = polygons;
        }

        public override string ToString()
        {
            return (m_ShapeName ?? "") + 
                   (Points != null ? string.Join("", Points.Select(p => p?.PointName)) : "");
        }
    }
}