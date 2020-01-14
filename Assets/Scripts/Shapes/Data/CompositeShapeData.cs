using System.Linq;
using Shapes.View;

namespace Shapes.Data
{
    public class CompositeShapeData : ShapeData
    {
        CompositeShapeView CompositeShapeView => View as CompositeShapeView;
        
        public readonly PointData[] Points;
        public readonly LineData[] Lines;
        public readonly PolygonData[] Polygons;

        private string m_ShapeName;

        public CompositeShapeData(PointData[] points, LineData[] lines, PolygonData[] polygons, string shapeName)
        {
            Points = points;
            Lines = lines;
            Polygons = polygons;

            m_ShapeName = shapeName;
        }

        public override string ToString()
        {
            return $"{m_ShapeName} {string.Join("", Points.Select(p => p.PointName))}";
        }
    }
}