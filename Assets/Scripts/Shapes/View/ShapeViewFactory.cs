using System.Linq;
using Shapes.Data;

namespace Shapes.View
{
    public class ShapeViewFactory
    {
        public PointView RequestPointView(PointData data)
        {
            return null;
        }
        
        public LineView RequestLineView(LineData data)
        {
            return null;
        }
        
        public PolygonView RequestPolygonView(PolygonData data)
        {
            return null;
        }

        public void ReturnView(IShapeView view)
        {
            
        }

        public CompositeShapeView RequestCompositeShapeView(CompositeShapeData data)
        {
            return new CompositeShapeView(
                data.Points.Select(p => p.PointView).ToArray(),
                data.Lines.Select(l => l.LineView).ToArray(),
                data.Polygons.Select(p => p.PolygonView).ToArray());
        }
    }
}