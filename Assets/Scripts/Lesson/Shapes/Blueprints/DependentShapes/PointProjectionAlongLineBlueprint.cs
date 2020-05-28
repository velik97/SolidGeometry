using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Lesson.Validators;
using Lesson.Validators.Polygon;
using Lesson.Validators.Projections;
using Newtonsoft.Json;
using Serialization;
using UnityEngine;
using Util;

namespace Lesson.Shapes.Blueprints.DependentShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointProjectionAlongLineBlueprint : ShapeBlueprint
    {
        [JsonProperty] public readonly PointData PointData;

        [JsonProperty] private PointData m_ProjectedPoint;
        [JsonProperty] private PointData m_FirstPointOnTargetLine;
        [JsonProperty] private PointData m_SecondPointOnTargetLine;
        [JsonProperty] private PointData m_FirstPointOnParallelLine;
        [JsonProperty] private PointData m_SecondPointOnParallelLine;
    
        public PointData ProjectedPoint => m_ProjectedPoint;
        public PointData FirstPointOnTargetLine => m_FirstPointOnTargetLine;
        public PointData SecondPointOnTargetLine => m_SecondPointOnTargetLine;
        public PointData FirstPointOnParallelLine => m_FirstPointOnParallelLine;
        public PointData SecondPointOnParallelLine => m_SecondPointOnParallelLine;
        
        public PointsNotSameValidator TargetPointsNotSameValidator;
        public PointsNotSameValidator ParallelPointsNotSameValidator;
        public ProjectionAlongLineValidator ProjectionAlongLineValidator;

        public override ShapeData MainShapeData => PointData;

        public PointProjectionAlongLineBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = ShapeDataFactory.CreatePointData();

            OnDeserialized();
        }

        [JsonConstructor]
        public PointProjectionAlongLineBlueprint(JsonConstructorMark _)
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

            if (m_SecondPointOnParallelLine != null)
            {
                m_SecondPointOnParallelLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }
            
            if (m_FirstPointOnParallelLine != null)
            {
                m_FirstPointOnParallelLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            RestoreDependencies();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            AddToMyShapeDatas(PointData);

            TargetPointsNotSameValidator = new PointsNotSameValidator(EnumerateTargetPoints());
            ParallelPointsNotSameValidator = new PointsNotSameValidator(EnumerateParallelPoints());
            
            PointData.NameUpdated.Subscribe(NameUpdated);

            ProjectionAlongLineValidator = new ProjectionAlongLineValidator(this);

            GeometryUpdated.Invoke();
        }

        private IEnumerable<PointData> EnumerateTargetPoints()
        {
            yield return m_ProjectedPoint;
            yield return m_FirstPointOnTargetLine;
            yield return m_SecondPointOnTargetLine;
        }
        
        private IEnumerable<PointData> EnumerateParallelPoints()
        {
            yield return m_ProjectedPoint;
            yield return m_FirstPointOnParallelLine;
            yield return m_SecondPointOnParallelLine;
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

        public void SetSecondPointTargetOnLine(PointData pointData)
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

        public void SetFirstPointOnParallelLine(PointData pointData)
        {
            if (m_FirstPointOnParallelLine == pointData)
            {
                return;
            }

            if (m_FirstPointOnParallelLine != null)
            {
                m_FirstPointOnParallelLine.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_FirstPointOnParallelLine = pointData;
            if (m_FirstPointOnParallelLine != null)
            {
                m_FirstPointOnParallelLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }
        public void SetSecondPointOnParallelLine(PointData pointData)
        {
            if (m_SecondPointOnParallelLine == pointData)
            {
                return;
            }

            if (m_SecondPointOnParallelLine != null)
            {
                m_SecondPointOnParallelLine.GeometryUpdated.Unsubscribe(GeometryUpdated);
            }

            m_SecondPointOnParallelLine = pointData;
            if (m_SecondPointOnParallelLine != null)
            {
                m_SecondPointOnParallelLine.GeometryUpdated.Subscribe(GeometryUpdated);
            }

            GeometryUpdated.Invoke();
        }

        protected override void UpdateGeometry()
        {
            TargetPointsNotSameValidator.Update();
            ParallelPointsNotSameValidator.Update();
            
            ProjectionAlongLineValidator.Update();

            if (!ParallelPointsNotSameValidator.IsValid() ||
                !TargetPointsNotSameValidator.IsValid())
            {
                return;
            }

            if (m_FirstPointOnTargetLine == null 
                || m_ProjectedPoint == null 
                || m_SecondPointOnTargetLine == null 
                || m_FirstPointOnParallelLine == null 
                || m_SecondPointOnParallelLine == null)
            {
                return;
            }
            
            if (!ProjectionAlongLineValidator.IsValid())
            {
                return;
            }

            Vector3 vAlong = m_SecondPointOnParallelLine.Position - m_FirstPointOnParallelLine.Position;

            Vector3 point = m_ProjectedPoint.Position + vAlong;

            PointData.SetPosition(GeometryUtils.PointOfIntersection(point, 
                m_ProjectedPoint.Position, 
                m_FirstPointOnTargetLine.Position, 
                m_SecondPointOnTargetLine.Position));
        }
    }
}
