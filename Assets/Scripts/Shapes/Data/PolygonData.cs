using System;
using System.Linq;
using Shapes.View;

namespace Shapes.Data
{
    [Serializable]
    public class PolygonData : ShapeData
    {
        public PolygonView PolygonView => View as PolygonView;
        
        public PointData[] Points;

        public PolygonData()
        {
        }

        public override string ToString()
        {
            return $"Polygon {string.Join("", Points.Select(p => p.PointName))}";
        }
    }
}