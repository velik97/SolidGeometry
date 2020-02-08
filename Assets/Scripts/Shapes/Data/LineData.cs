using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Shapes.Validators;
using Shapes.Validators.Line;
using Shapes.View;

namespace Shapes.Data
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class LineData : ShapeData
    {
        public LineView LineView => View as LineView;
        
        public PointData StartPoint => m_StartPoint;
        public PointData EndPoint => m_EndPoint;

        public LineUniquenessValidator UniquenessValidator;
        public PointsNotSameValidator PointsNotSameValidator;
        
        [JsonProperty]
        private PointData m_StartPoint;
        [JsonProperty]
        private PointData m_EndPoint;

        public LineData()
        {
            UniquenessValidator = new LineUniquenessValidator(this);
            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            
            NameUpdated += PointsNotSameValidator.Update;
        }

        [JsonConstructor]
        public LineData(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            SubscribeOnPoint(m_StartPoint);
            SubscribeOnPoint(m_EndPoint);
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            UniquenessValidator = new LineUniquenessValidator(this);
            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            
            NameUpdated += PointsNotSameValidator.Update;
            PointsNotSameValidator.Update();
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
            SubscribeOnPoint(m_EndPoint);
        }

        public override string ToString()
        {
            return $"Line {m_StartPoint?.PointName}{m_EndPoint?.PointName}";
        }
    }
}