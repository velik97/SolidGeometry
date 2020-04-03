using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Lesson.Validators;
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
        [JsonProperty] private PointData m_SecondPointOnLine;
        [JsonProperty] private PointData m_FirstPointOnLine;
        [JsonProperty] private PointData m_FirstPointAlong;
        [JsonProperty] private PointData m_SecondPointAlong;
    
        public PointData ProjectedPoint => m_ProjectedPoint;
        public PointData SecondPointOnLine => m_SecondPointOnLine;
        public PointData FirstPointOnLine => m_FirstPointOnLine;
        public PointData FirstPointAlong => m_FirstPointAlong;
        public PointData SecondPointAlong => m_SecondPointAlong;
    

        public PointsNotSameValidator PointsNotSameValidator;

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
            if (m_FirstPointOnLine != null)
            {
                m_FirstPointOnLine.GeometryUpdated += UpdatePosition;
            }

            if (m_SecondPointOnLine != null)
            {
                m_SecondPointOnLine.GeometryUpdated += UpdatePosition;
            }
            
            if (m_ProjectedPoint != null)
            {
                m_ProjectedPoint.GeometryUpdated += UpdatePosition;
            }

            if (m_SecondPointAlong != null)
            {
                m_SecondPointAlong.GeometryUpdated += UpdatePosition;
            }
            
            if (m_FirstPointAlong != null)
            {
                m_FirstPointAlong.GeometryUpdated += UpdatePosition;
            }
            

            RestoreDependencies();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            PointData.NameUpdated += OnNameUpdated;

            UpdatePosition();
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            yield return m_ProjectedPoint;
            yield return m_SecondPointOnLine;
            yield return m_FirstPointOnLine;
            yield return m_FirstPointAlong;
            yield return m_SecondPointAlong;
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

        public void SetSecondPointOnLine(PointData pointData)
        {
            if (m_SecondPointOnLine == pointData)
            {
                return;
            }

            if (m_SecondPointOnLine != null)
            {
                m_SecondPointOnLine.GeometryUpdated -= UpdatePosition;
            }

            m_SecondPointOnLine = pointData;
            if (m_SecondPointOnLine != null)
            {
                m_SecondPointOnLine.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }

        public void SetFirstPointOnLine(PointData pointData)
        {
            if (m_FirstPointOnLine == pointData)
            {
                return;
            }

            if (m_FirstPointOnLine != null)
            {
                m_FirstPointOnLine.GeometryUpdated -= UpdatePosition;
            }

            m_FirstPointOnLine = pointData;
            if (m_FirstPointOnLine != null)
            {
                m_FirstPointOnLine.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }
        
        public void SetFirstPointAlong(PointData pointData)
        {
            if (m_FirstPointAlong == pointData)
            {
                return;
            }

            if (m_FirstPointAlong != null)
            {
                m_FirstPointAlong.GeometryUpdated -= UpdatePosition;
            }

            m_FirstPointAlong = pointData;
            if (m_FirstPointAlong != null)
            {
                m_FirstPointAlong.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }
        public void SetSecondPointAlong(PointData pointData)
        {
            if (m_SecondPointAlong == pointData)
            {
                return;
            }

            if (m_SecondPointAlong != null)
            {
                m_SecondPointAlong.GeometryUpdated -= UpdatePosition;
            }

            m_SecondPointAlong = pointData;
            if (m_SecondPointAlong != null)
            {
                m_SecondPointAlong.GeometryUpdated += UpdatePosition;
            }

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            PointsNotSameValidator.Update();
            if (!PointsNotSameValidator.IsValid())
            {
                return;
            }

            if (m_FirstPointOnLine == null || m_ProjectedPoint == null || m_SecondPointOnLine == null || m_FirstPointAlong == null || m_SecondPointAlong == null)
            {
                return;
            }

            Vector3 vOn = m_SecondPointOnLine.Position - m_FirstPointOnLine.Position;
            Vector3 vAlong = m_SecondPointAlong.Position - m_FirstPointAlong.Position;
            Vector3 v = m_ProjectedPoint.Position - m_FirstPointOnLine.Position;

            Vector3 point = m_ProjectedPoint.Position + vAlong;
            //Write that not collinear???
            if (Vector3Extensions.CollinearWith(vOn, vAlong, v))
            {
                PointData.SetPosition(GeometryUtils.PointOfIntersection(point, 
                    m_ProjectedPoint.Position, 
                    m_FirstPointOnLine.Position, 
                    m_SecondPointOnLine.Position));
            }
            else
            {
                throw new ArgumentException("lines don't collinear");
            }
            
            
        }
    }
}
