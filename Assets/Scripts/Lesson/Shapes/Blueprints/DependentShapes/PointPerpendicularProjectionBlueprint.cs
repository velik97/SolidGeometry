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
        private PointData m_SourcePointData;
        [JsonProperty]
        private PointData m_FirstPointOnLine;
        [JsonProperty]
        private PointData m_SecondPointOnLine;

        public PointData SourcePointData => m_SourcePointData;
        public PointData FirstPointOnLine => m_FirstPointOnLine;
        public PointData SecondPointOnLine => m_SecondPointOnLine;

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
            if (m_SourcePointData != null)
            {
                m_SourcePointData.GeometryUpdated += UpdatePosition;
            }
            if (m_FirstPointOnLine != null)
            {
                m_FirstPointOnLine.GeometryUpdated += UpdatePosition;
            }
            if (m_SecondPointOnLine != null)
            {
                m_SecondPointOnLine.GeometryUpdated += UpdatePosition;
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
            yield return m_SourcePointData;
            yield return m_FirstPointOnLine;
            yield return m_SecondPointOnLine;
        }

        public void SetSourcePoint(PointData pointData)
        {
            if (m_SourcePointData == pointData)
            {
                return;
            }
            if (m_SourcePointData != null)
            {
                m_SourcePointData.GeometryUpdated -= UpdatePosition;
            }
            m_SourcePointData = pointData;
            if (m_SourcePointData != null)
            {
                m_SourcePointData.GeometryUpdated += UpdatePosition;
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

        private void UpdatePosition()
        {
            PointsNotSameValidator.Update();
            if (!PointsNotSameValidator.IsValid())
            {
                return;
            }
            if (m_SourcePointData == null || m_FirstPointOnLine == null || m_SecondPointOnLine == null)
            {
                return;
            }
            PointData.SetPosition(GeometryUtils.ProjectionOnLine(
                m_SourcePointData.Position,
                m_FirstPointOnLine.Position,
                m_SecondPointOnLine.Position));
        }
    }
}