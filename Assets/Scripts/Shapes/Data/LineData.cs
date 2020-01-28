using System;
using System.Collections.Generic;
using Shapes.Validators;
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

        public readonly LineUniquenessValidator UniquenessValidator;
        public readonly PointsNotSameValidator PointsNotSameValidator;
        
        private PointData m_StartPoint;
        private PointData m_EndPoint;

        public LineData()
        {
            UniquenessValidator = new LineUniquenessValidator(this);
            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            
            NameUpdated += PointsNotSameValidator.Update;
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            yield return m_StartPoint;
            yield return m_EndPoint;
        }

        public void SetStartPoint(PointData pointData)
        {
            if (m_StartPoint == pointData)
            {
                return;
            }
            UnsubscribeFromPoint(m_StartPoint);
            m_StartPoint = pointData;
            SubscribeOnPoint(m_StartPoint);
        }
        
        public void SetEndPoint(PointData pointData)
        {
            if (m_EndPoint == pointData)
            {
                return;
            }
            UnsubscribeFromPoint(m_EndPoint);
            m_EndPoint = pointData;
            SubscribeOnPoint(EndPoint);
        }

        public override string ToString()
        {
            return $"Line {m_StartPoint?.PointName}{m_EndPoint?.PointName}";
        }
    }
}