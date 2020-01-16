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

        public override string ToString()
        {
            return $"{m_ShapeName} {string.Join("", Points.Select(p => p.PointName))}";
        }
    }
}