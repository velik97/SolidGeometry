using System.Collections.Generic;
using Shapes.Data;
using Shapes.Validators;
using Shapes.Validators.PointOfIntersection;
using Util;

namespace Shapes.Blueprint.DependentShapes
{
    public class PointOfIntersectionBlueprint : ShapeBlueprint
    {
        public readonly PointData PointData;

        private PointData[][] m_PointsOnLines = new PointData[2][]; // m_PointsOnLines[lineNum][pointNum]

        public PointData[][] PointsOnLines => m_PointsOnLines;

        public readonly PointsNotSameValidator PointsNotSameValidator;
        public readonly LinesIntersectValidator LinesIntersectValidator;

        public override ShapeData MainShapeData => PointData;

        public PointOfIntersectionBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = DataFactory.CreatePointData();
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            PointData.NameUpdated += OnNameUpdated;
            
            m_PointsOnLines[0] = new PointData[2];
            m_PointsOnLines[1] = new PointData[2];

            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            LinesIntersectValidator = new LinesIntersectValidator(this);
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            yield return m_PointsOnLines[0][0];
            yield return m_PointsOnLines[0][1];
            yield return m_PointsOnLines[1][0];
            yield return m_PointsOnLines[1][1];
        }

        public void SetPoint(int lineNum, int pointNum, PointData pointData)
        {
            if (m_PointsOnLines[lineNum][pointNum] == pointData)
            {
                return;
            }

            if (m_PointsOnLines[lineNum][pointNum] != null)
            {
                m_PointsOnLines[lineNum][pointNum].GeometryUpdated -= UpdatePosition;
            }

            m_PointsOnLines[lineNum][pointNum] = pointData;
            if (m_PointsOnLines[lineNum][pointNum] != null)
            {
                m_PointsOnLines[lineNum][pointNum].GeometryUpdated += UpdatePosition;
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

            if (m_PointsOnLines[0][0] == null
                || m_PointsOnLines[0][1] == null
                || m_PointsOnLines[1][0] == null
                || m_PointsOnLines[1][1] == null)
            {
                return;
            }
            
            LinesIntersectValidator.Update();
            if (!LinesIntersectValidator.IsValid())
            {
                return;
            }

            PointData.SetPosition(GeometryUtils.PointOfIntersection(
                m_PointsOnLines[0][0].Position,
                m_PointsOnLines[0][1].Position,
                m_PointsOnLines[1][0].Position,
                m_PointsOnLines[1][1].Position));
        }
    }
}