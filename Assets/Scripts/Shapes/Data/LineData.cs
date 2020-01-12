using Shapes.View;

namespace Shapes.Data
{
    public class LineData : ShapeData
    {
        public LineView LineView => View as LineView;
        
        public readonly PointData StartPoint;
        public readonly PointData EndPoint;

        public LineData(PointData startPoint, PointData endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override string ToString()
        {
            return $"Line: {StartPoint.PointName}{EndPoint.PointName}";
        }
    }
}