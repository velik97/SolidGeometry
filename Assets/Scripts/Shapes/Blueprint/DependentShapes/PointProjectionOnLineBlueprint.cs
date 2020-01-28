using System.Collections.Generic;
using Shapes.Data;
using Shapes.Validators;
using Util;

namespace Shapes.Blueprint.DependentShapes
{
    public class PointProjectionOnLineBlueprint : ShapeBlueprint
    {
        public readonly PointData PointData;

        private PointData m_SourcePointData;
        private PointData m_FirstPointOnLine;
        private PointData m_SecondPointOnLine;

        public PointData SourcePointData => m_SourcePointData;
        public PointData FirstPointOnLine => m_FirstPointOnLine;
        public PointData SecondPointOnLine => m_SecondPointOnLine;

        public PointsNotSameValidator PointsNotSameValidator;

        public override ShapeData MainShapeData => PointData;
        
        public PointProjectionOnLineBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = DataFactory.CreatePointData();
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            PointData.NameUpdated += OnNameUpdated;
            
            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
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