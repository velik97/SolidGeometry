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
                m_FirstPointOnTargetLine.GeometryUpdated += UpdatePosition;
            }

            if (m_SecondPointOnTargetLine != null)
            {
                m_SecondPointOnTargetLine.GeometryUpdated += UpdatePosition;
            }
            
            if (m_ProjectedPoint != null)
            {
                m_ProjectedPoint.GeometryUpdated += UpdatePosition;
            }

            if (m_SecondPointOnParallelLine != null)
            {
                m_SecondPointOnParallelLine.GeometryUpdated += UpdatePosition;
            }
            
            if (m_FirstPointOnParallelLine != null)
            {
                m_FirstPointOnParallelLine.GeometryUpdated += UpdatePosition;
            }

            RestoreDependencies();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            TargetPointsNotSameValidator = new PointsNotSameValidator(EnumerateTargetPoints());
            ParallelPointsNotSameValidator = new PointsNotSameValidator(EnumerateParallelPoints());
            
            PointData.NameUpdated += OnNameUpdated;

            ProjectionAlongLineValidator = new ProjectionAlongLineValidator(this);

            UpdatePosition();
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
                m_ProjectedPoint.GeometryUpdated -= UpdatePosition;
            }

            m_ProjectedPoint = pointData;
            if (m_ProjectedPoint != null)
            {
                m_ProjectedPoint.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }
        
        public void SetFirstPointOnTargetLine(PointData pointData)
        {
            if (m_FirstPointOnTargetLine == pointData)
            {
                return;
            }

            if (m_FirstPointOnTargetLine != null)
            {
                m_FirstPointOnTargetLine.GeometryUpdated -= UpdatePosition;
            }

            m_FirstPointOnTargetLine = pointData;
            if (m_FirstPointOnTargetLine != null)
            {
                m_FirstPointOnTargetLine.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }

        public void SetSecondPointTargetOnLine(PointData pointData)
        {
            if (m_SecondPointOnTargetLine == pointData)
            {
                return;
            }

            if (m_SecondPointOnTargetLine != null)
            {
                m_SecondPointOnTargetLine.GeometryUpdated -= UpdatePosition;
            }

            m_SecondPointOnTargetLine = pointData;
            if (m_SecondPointOnTargetLine != null)
            {
                m_SecondPointOnTargetLine.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }

        public void SetFirstPointOnParallelLine(PointData pointData)
        {
            if (m_FirstPointOnParallelLine == pointData)
            {
                return;
            }

            if (m_FirstPointOnParallelLine != null)
            {
                m_FirstPointOnParallelLine.GeometryUpdated -= UpdatePosition;
            }

            m_FirstPointOnParallelLine = pointData;
            if (m_FirstPointOnParallelLine != null)
            {
                m_FirstPointOnParallelLine.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }
        public void SetSecondPointOnParallelLine(PointData pointData)
        {
            if (m_SecondPointOnParallelLine == pointData)
            {
                return;
            }

            if (m_SecondPointOnParallelLine != null)
            {
                m_SecondPointOnParallelLine.GeometryUpdated -= UpdatePosition;
            }

            m_SecondPointOnParallelLine = pointData;
            if (m_SecondPointOnParallelLine != null)
            {
                m_SecondPointOnParallelLine.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }

        private void UpdatePosition()
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
