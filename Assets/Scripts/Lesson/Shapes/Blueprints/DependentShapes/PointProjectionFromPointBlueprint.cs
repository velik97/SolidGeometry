using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Lesson.Validators;
using Newtonsoft.Json;
using Serialization;
using Util;

namespace Lesson.Shapes.Blueprints.DependentShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointProjectionFromPointBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        public readonly PointData PointData;

        [JsonProperty]
        private PointData m_ProjectedPoint;
        [JsonProperty]
        private PointData m_SourcePoint;
        [JsonProperty]
        private PointData m_FirstPointOnTargetLine;
        [JsonProperty]
        private PointData m_SecondPointOnTargetLine;

        public PointData ProjectedPoint => m_ProjectedPoint;
        public PointData FirstPointOnTargetLine => m_FirstPointOnTargetLine;
        public PointData SecondPointOnTargetLine => m_SecondPointOnTargetLine;

        public PointData SourcePoint => m_SourcePoint;
    

        public PointsNotSameValidator PointsNotSameValidator;

        public override ShapeData MainShapeData => PointData;

        public PointProjectionFromPointBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = ShapeDataFactory.CreatePointData();

            OnDeserialized();
        }

        [JsonConstructor]
        public PointProjectionFromPointBlueprint(JsonConstructorMark _)
        {
        }

        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            if (m_FirstPointOnTargetLine != null)
            {
                m_FirstPointOnTargetLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            if (m_SecondPointOnTargetLine != null)
            {
                m_SecondPointOnTargetLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }
            
            if (m_ProjectedPoint != null)
            {
                m_ProjectedPoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            if (m_SourcePoint != null)
            {
                m_SourcePoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }
            
            RestoreDependencies();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            AddToMyShapeDatas(PointData);

            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            PointData.NameUpdated.Subscribe(NameUpdated);

            GeometryUpdated.Invoke();
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            yield return m_ProjectedPoint;
            yield return m_SecondPointOnTargetLine;
            yield return m_FirstPointOnTargetLine;
            yield return m_SourcePoint;
        }

        public void SetProjectedPoint(PointData pointData)
        {
            if (m_ProjectedPoint == pointData)
            {
                return;
            }

            if (m_ProjectedPoint != null)
            {
                m_ProjectedPoint.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_ProjectedPoint = pointData;
            if (m_ProjectedPoint != null)
            {
                m_ProjectedPoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }
        
        public void SetSourcePoint(PointData pointData)
        {
            if (m_SourcePoint == pointData)
            {
                return;
            }

            if (m_SourcePoint != null)
            {
                m_SourcePoint.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_SourcePoint = pointData;
            if (m_SourcePoint != null)
            {
                m_SourcePoint.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }
        
        public void SetFirstPointOnTargetLine(PointData pointData)
        {
            if (m_FirstPointOnTargetLine == pointData)
            {
                return;
            }

            if (m_FirstPointOnTargetLine != null)
            {
                m_FirstPointOnTargetLine.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_FirstPointOnTargetLine = pointData;
            if (m_FirstPointOnTargetLine != null)
            {
                m_FirstPointOnTargetLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }

        public void SetSecondPointOnTargetLine(PointData pointData)
        {
            if (m_SecondPointOnTargetLine == pointData)
            {
                return;
            }

            if (m_SecondPointOnTargetLine != null)
            {
                m_SecondPointOnTargetLine.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_SecondPointOnTargetLine = pointData;
            if (m_SecondPointOnTargetLine != null)
            {
                m_SecondPointOnTargetLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }

        protected override void UpdateGeometry()
        {
            PointsNotSameValidator.Update();
            if (!PointsNotSameValidator.IsValid())
            {
                return;
            }

            if (m_FirstPointOnTargetLine == null || m_ProjectedPoint == null || m_SecondPointOnTargetLine == null || m_SourcePoint == null )
            {
                return;
            }
            
            PointData.SetPosition(GeometryUtils.PointOfIntersection(m_SourcePoint.Position, 
                m_ProjectedPoint.Position, 
                m_FirstPointOnTargetLine.Position, 
                m_SecondPointOnTargetLine.Position));
        }
    }
}

