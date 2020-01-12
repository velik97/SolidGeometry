using System.Linq;
using Shapes.View;

namespace Shapes.Data
{
    public class PolygonData : ShapeData
    {
        public PolygonView PolygonView => View as PolygonView;
        
        public readonly PointData[] Points;

        public PolygonData(params PointData[] points)
        {
            Points = points;
        }

        public override string ToString()
        {
            return $"Polygon {string.Join("", Points.Select(p => p.PointName))}";
        }
    }
}