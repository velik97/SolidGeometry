using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Shapes.Data;
using Shapes.Validators;
using Shapes.Validators.PointOfIntersection;
using Util;

namespace Shapes.Blueprint.DependentShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PointOfIntersectionBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        public readonly PointData PointData;

        [JsonProperty]
        private PointData[][] m_PointsOnLines = new PointData[2][]; // m_PointsOnLines[lineNum][pointNum]

        public PointData[][] PointsOnLines => m_PointsOnLines;

        public PointsNotSameValidator PointsNotSameValidator;
        public LinesIntersectValidator LinesIntersectValidator;

        public override ShapeData MainShapeData => PointData;

        public PointOfIntersectionBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            PointData = DataFactory.CreatePointData();

            m_PointsOnLines[0] = new PointData[2];
            m_PointsOnLines[1] = new PointData[2];
            
            OnDeserialized();
        }
        
        [JsonConstructor]
        public PointOfIntersectionBlueprint(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            for (int i = 0; i < m_PointsOnLines.Length; i++)
            {
                if (m_PointsOnLines[i] == null) continue;
                for (int j = 0; j < m_PointsOnLines[i].Length; j++)
                {
                    if (m_PointsOnLines[i][j] == null) continue;
                    m_PointsOnLines[i][j].GeometryUpdated += UpdatePosition;
                }
            }
            RestoreDependences();
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            PointData.SourceBlueprint = this;
            MyShapeDatas.Add(PointData);

            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            LinesIntersectValidator = new LinesIntersectValidator(this);
            PointData.NameUpdated += OnNameUpdated;

            UpdatePosition();
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