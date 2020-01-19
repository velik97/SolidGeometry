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
        
        public PointData StartPoint => m_StartPoint;
        public PointData EndPoint => m_EndPoint;

        public LineUniquenessValidator UniquenessValidator;
        public LinePointsNotSameValidator m_PointsNotSameValidator;
        
        private PointData m_StartPoint;
        private PointData m_EndPoint;

        public LineData()
        {
            UniquenessValidator = new LineUniquenessValidator(this);
            m_PointsNotSameValidator = new LinePointsNotSameValidator(this);
        }

        public void SetStartPoint(PointData pointData)
        {
            if (m_StartPoint == pointData)
            {
                return;
            }
            if (m_StartPoint != null)
            {
                m_StartPoint.NameUpdated -= OnNameUpdated;
                m_StartPoint.GeometryUpdated -= OnGeometryUpdated;
            }
            m_StartPoint = pointData;
            if (m_StartPoint != null)
            {
                m_StartPoint.NameUpdated += OnNameUpdated;
                m_StartPoint.GeometryUpdated += OnGeometryUpdated;
            }
            OnNameUpdated();
            OnGeometryUpdated();
        }
        
        public void SetEndPoint(PointData pointData)
        {
            if (m_EndPoint == pointData)
            {
                return;
            }
            if (m_EndPoint != null)
            {
                m_EndPoint.NameUpdated -= OnNameUpdated;
                m_EndPoint.GeometryUpdated -= OnGeometryUpdated;
            }
            m_EndPoint = pointData;
            if (m_EndPoint != null)
            {
                m_EndPoint.NameUpdated += OnNameUpdated;
                m_EndPoint.GeometryUpdated += OnGeometryUpdated;
            }
            OnNameUpdated();
            OnGeometryUpdated();
        }

        public override string ToString()
        {
            return $"Line {m_StartPoint?.PointName}{m_EndPoint?.PointName}";
        }
    }
}