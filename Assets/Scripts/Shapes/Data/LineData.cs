using System;
using Shapes.Validators.Line;
using Shapes.Validators.Point;
using Shapes.View;

namespace Shapes.Data
{
    [Serializable]
    public class LineData : ShapeData
    {
        public LineView LineView => View as LineView;
        
        public PointData StartPoint;
        public PointData EndPoint;

        public LineUniquenessValidator UniquenessValidator;
        public LinePointsNotSameValidator m_PointsNotSameValidator;

        public LineData()
        {
            UniquenessValidator = new LineUniquenessValidator(this);
            m_PointsNotSameValidator = new LinePointsNotSameValidator(this);
        }

        public void SetStartPoint(PointData pointData)
        {
            if (StartPoint == pointData)
            {
                return;
            }
            if (StartPoint != null)
            {
                StartPoint.NameUpdated -= OnNameUpdated;
                StartPoint.GeometryUpdated -= OnGeometryUpdated;
            }
            StartPoint = pointData;
            if (StartPoint != null)
            {
                StartPoint.NameUpdated += OnNameUpdated;
                StartPoint.GeometryUpdated += OnGeometryUpdated;
            }
            OnNameUpdated();
            OnGeometryUpdated();
        }
        
        public void SetEndPoint(PointData pointData)
        {
            if (EndPoint == pointData)
            {
                return;
            }
            if (EndPoint != null)
            {
                EndPoint.NameUpdated -= OnNameUpdated;
                EndPoint.GeometryUpdated -= OnGeometryUpdated;
            }
            EndPoint = pointData;
            if (EndPoint != null)
            {
                EndPoint.NameUpdated += OnNameUpdated;
                EndPoint.GeometryUpdated += OnGeometryUpdated;
            }
            OnNameUpdated();
            OnGeometryUpdated();
        }

        public override string ToString()
        {
            return $"Line {StartPoint?.PointName}{EndPoint?.PointName}";
        }
    }
}