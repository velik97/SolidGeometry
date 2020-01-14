using Shapes.View;

namespace Shapes.Data
{
    public class LineData : ShapeData
    {
        public LineView LineView => View as LineView;
        
        public PointData StartPoint;
        public PointData EndPoint;

        public void SetStartPoint(PointData pointData)
        {
            StartPoint = pointData;
        }
        
        public void SetEndPoint(PointData pointData)
        {
            EndPoint = pointData;
        }

        public override string ToString()
        {
            return $"Line {StartPoint.PointName}{EndPoint.PointName}";
        }
    }
}