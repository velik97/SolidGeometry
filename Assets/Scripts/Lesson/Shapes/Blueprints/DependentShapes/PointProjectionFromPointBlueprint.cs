using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Lesson.Validators;
using Newtonsoft.Json;
using Serialization;
using UnityEngine;
using Util;

    //public class PointProjectionContinuationOfPointBlueprint


namespace Lesson.Shapes.Blueprints.DependentShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointProjectionFromPointBlueprint : ShapeBlueprint
    {
        [JsonProperty] public readonly PointData PointData;

        [JsonProperty] private PointData m_ProjectedPoint;
        [JsonProperty] private PointData m_SecondPointOnLine;
        [JsonProperty] private PointData m_FirstPointOnLine;
        [JsonProperty] private PointData m_PointAlong;
        public PointData ProjectedPoint => m_ProjectedPoint;
        public PointData SecondPointOnLine => m_SecondPointOnLine;
        public PointData FirstPointOnLine => m_FirstPointOnLine;
        public PointData PointAlong => m_PointAlong;
    

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

            if (m_PointAlong != null)
            {
                m_PointAlong.GeometryUpdated += UpdatePosition;
            }
            
            RestoreDependences();
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
            yield return m_PointAlong;
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
        
        public void SetPointAlong(PointData pointData)
        {
            if (m_PointAlong == pointData)
            {
                return;
            }

            if (m_PointAlong != null)
            {
                m_PointAlong.GeometryUpdated -= UpdatePosition;
            }

            m_PointAlong = pointData;
            if (m_PointAlong != null)
            {
                m_PointAlong.GeometryUpdated += UpdatePosition;
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

            if (m_FirstPointOnLine == null || m_ProjectedPoint == null || m_SecondPointOnLine == null || m_PointAlong == null )
            {
                return;
            }
            
            PointData.SetPosition(GeometryUtils.PointOfIntersection(m_PointAlong.Position, 
                m_ProjectedPoint.Position, 
                m_FirstPointOnLine.Position, 
                m_SecondPointOnLine.Position));
        }
    }
}

