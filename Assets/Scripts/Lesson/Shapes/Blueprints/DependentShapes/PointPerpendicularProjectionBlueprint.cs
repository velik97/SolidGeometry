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
    public class PointPerpendicularProjectionBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        public readonly PointData PointData;

        [JsonProperty]
        private PointData m_ProjectedPoint;
        [JsonProperty]
        private PointData m_FirstPointOnTargetLine;
        [JsonProperty]
        private PointData m_SecondPointOnTargetLine;

        public PointData ProjectedPoint => m_ProjectedPoint;
        public PointData FirstPointOnTargetLine => m_FirstPointOnTargetLine;
        public PointData SecondPointOnTargetLine => m_SecondPointOnTargetLine;

        public PointsNotSameValidator PointsNotSameValidator;

        public override ShapeData MainShapeData => PointData;
        
        public PointPerpendicularProjectionBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = ShapeDataFactory.CreatePointData();

            OnDeserialized();
        }
        
        [JsonConstructor]
        public PointPerpendicularProjectionBlueprint(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            if (m_ProjectedPoint != null)
            {
                m_ProjectedPoint.GeometryUpdated += UpdatePosition;
            }
            if (m_FirstPointOnTargetLine != null)
            {
                m_FirstPointOnTargetLine.GeometryUpdated += UpdatePosition;
            }
            if (m_SecondPointOnTargetLine != null)
            {
                m_SecondPointOnTargetLine.GeometryUpdated += UpdatePosition;
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
            yield return m_FirstPointOnTargetLine;
            yield return m_SecondPointOnTargetLine;
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
        
        public void SetSecondPointOnTargetLine(PointData pointData)
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

        private void UpdatePosition()
        {
            PointsNotSameValidator.Update();
            if (!PointsNotSameValidator.IsValid())
            {
                return;
            }
            if (m_ProjectedPoint == null || m_FirstPointOnTargetLine == null || m_SecondPointOnTargetLine == null)
            {
                return;
            }
            PointData.SetPosition(m_ProjectedPoint.Position.ProjectionOnLine(
                m_FirstPointOnTargetLine.Position,
                m_SecondPointOnTargetLine.Position));
        }
    }
}