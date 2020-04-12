using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Newtonsoft.Json;
using UnityEngine;

namespace Lesson.Shapes.Blueprints.CompositeShapes
{
    public class PrismBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        private int m_VerticesAtTheBaseCount = 3;
        [JsonProperty]
        private Vector3 m_Origin;
        [JsonProperty]
        private Vector3 m_Offset;
        [JsonProperty]
        private List<Vector3> m_PointsPositions = new List<Vector3>();

        [JsonProperty]
        private readonly CompositeShapeData m_CompositeShapeData;

        [JsonProperty]
        private readonly List<PointData> m_Points = new List<PointData>();
        [JsonProperty]
        private readonly List<LineData> m_Lines = new List<LineData>();
        [JsonProperty]
        private readonly List<PolygonData> m_Polygons = new List<PolygonData>();

        public int VerticesAtTheBaseCount => m_VerticesAtTheBaseCount;

        public Vector3 Origin => m_Origin;
        public Vector3 Offset => m_Offset;
        public IReadOnlyList<Vector3> PointsPositions => m_PointsPositions;

        public IReadOnlyList<PointData> Points => m_Points;
        public IReadOnlyList<LineData> Lines => m_Lines;
        public IReadOnlyList<PolygonData> Polygons => m_Polygons;

        public override ShapeData MainShapeData => m_CompositeShapeData;
        
        public PrismBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            m_CompositeShapeData = dataFactory.CreateCompositeShapeData();
            ConstructPrism();
            OnDeserialized();
        }

        [JsonConstructor]
        public PrismBlueprint(object _)
        {
        }

        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (ShapeData shapeData in
                new[] {m_CompositeShapeData}.Cast<ShapeData>()
                    .Concat(m_Points)
                    .Concat(m_Lines)
                    .Concat(m_Polygons))
            {
                MyShapeDatas.Add(shapeData);
                shapeData.SourceBlueprint = this;
            }

            OnDeserialized();
        }

        private void OnDeserialized()
        {
            UpdatePointsPositions();

            m_CompositeShapeData.SetShapeName("Prism");
        }

        private void ConstructPrism()
        {
            UpdateVerticesPositionsList();
            
            for (int i = 0; i < 2 * m_VerticesAtTheBaseCount; i++)
            {
                m_Points.Add(ShapeDataFactory.CreatePointData());
                m_Points[i].NameUpdated += OnNameUpdated;
            }

            for (int i = 0; i < 3 * m_VerticesAtTheBaseCount; i++)
            {
                m_Lines.Add(ShapeDataFactory.CreateLineData());
            }

            for (int i = 0; i < m_VerticesAtTheBaseCount + 2; i++)
            {
                m_Polygons.Add(ShapeDataFactory.CreatePolygonData());
                // Side faces
                if (i < m_VerticesAtTheBaseCount)
                {
                    m_Polygons[i].SetPointsCount(4);
                }
                // Top and Bottom faces
                else
                {
                    m_Polygons[i].SetPointsCount(m_VerticesAtTheBaseCount);
                }
            }

            ConstructLines();
            ConstructPolygons();

            m_CompositeShapeData.SetPoints(m_Points.ToArray());
            m_CompositeShapeData.SetLines(m_Lines.ToArray());
            m_CompositeShapeData.SetPolygons(m_Polygons.ToArray());

            foreach (ShapeData shapeData in
                new[] {m_CompositeShapeData}.Cast<ShapeData>()
                    .Concat(m_Points)
                    .Concat(m_Lines)
                    .Concat(m_Polygons))
            {
                MyShapeDatas.Add(shapeData);
                shapeData.SourceBlueprint = this;
            }
        }

        private void UpdateVerticesPositionsList()
        {
            while (m_PointsPositions.Count < m_VerticesAtTheBaseCount)
            {
                m_PointsPositions.Add(Vector3.zero);
            }
            
            while (m_PointsPositions.Count > m_VerticesAtTheBaseCount)
            {
                m_PointsPositions.RemoveAt(m_PointsPositions.Count - 1);
            }
        }

        private void ConstructLines()
        {
            // Bottom edges
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Lines[i].SetStartPoint(m_Points[i % m_VerticesAtTheBaseCount]);
                m_Lines[i].SetEndPoint(m_Points[(i + 1) % m_VerticesAtTheBaseCount]);
            }

            // Top edges
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Lines[i + m_VerticesAtTheBaseCount]
                    .SetStartPoint(m_Points[(i % m_VerticesAtTheBaseCount) + m_VerticesAtTheBaseCount]);
                m_Lines[i + m_VerticesAtTheBaseCount]
                    .SetEndPoint(m_Points[((i + 1) % m_VerticesAtTheBaseCount) + m_VerticesAtTheBaseCount]);
            }

            // Side edges
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Lines[i + 2 * m_VerticesAtTheBaseCount].SetStartPoint(m_Points[i]);
                m_Lines[i + 2 * m_VerticesAtTheBaseCount].SetEndPoint(m_Points[i + m_VerticesAtTheBaseCount]);
            }
        }

        private void ConstructPolygons()
        {
            // Top face
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Polygons[m_VerticesAtTheBaseCount + 1].SetPoint(i, m_Points[i + m_VerticesAtTheBaseCount]);
            }

            // Bottom face
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Polygons[m_VerticesAtTheBaseCount].SetPoint(i, m_Points[i]);
            }

            // Side faces
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Polygons[i].SetPoint(0, m_Points[i]);
                m_Polygons[i].SetPoint(1, m_Points[m_VerticesAtTheBaseCount + i]);

                m_Polygons[i].SetPoint(2, m_Points[((i + 1) % m_VerticesAtTheBaseCount) + m_VerticesAtTheBaseCount]);
                m_Polygons[i].SetPoint(3, m_Points[(i + 1) % m_VerticesAtTheBaseCount]);
            }
        }

        private void Clear()
        {
            foreach (ShapeData shapeData in MyShapeDatas)
            {
                shapeData.SourceBlueprint = null;
            }

            MyShapeDatas.Clear();

            foreach (PointData pointData in m_Points)
            {
                pointData.NameUpdated -= OnNameUpdated;
                ShapeDataFactory.RemoveShapeData(pointData);
            }

            m_Points.Clear();

            foreach (LineData lineData in m_Lines)
            {
                ShapeDataFactory.RemoveShapeData(lineData);
            }

            m_Lines.Clear();

            foreach (PolygonData polygonData in m_Polygons)
            {
                ShapeDataFactory.RemoveShapeData(polygonData);
            }

            m_Polygons.Clear();
        }

        public void SetPointName(int pointIndex, string name)
        {
            if (pointIndex < 0 || pointIndex >= m_Points.Count)
            {
                return;
            }

            m_Points[pointIndex].SetName(name);
        }

        public void SetOrigin(Vector3 origin)
        {
            if (m_Origin == origin)
            {
                return;
            }

            m_Origin = origin;
            UpdatePointsPositions();
        }

        public void SetBaseVerticesCount(int count)
        {
            if (count > 8 || count < 3)
            {
                return;
            }

            m_VerticesAtTheBaseCount = count;

            Clear();
            ConstructPrism();

            UpdatePointsPositions();
        }

        public void SetOffset(Vector3 offset)
        {
            m_Offset = offset;
            UpdatePointsPositions();
        }
        
        public void SetPointPosition(int pointIndex, Vector3 position)
        {
            if (pointIndex < 0 || pointIndex >= m_PointsPositions.Count)
            {
                return;
            }

            m_PointsPositions[pointIndex] = position;
            UpdatePointsPositions();
        }

        private void UpdatePointsPositions()
        {
            for (int i = 0; i < m_PointsPositions.Count; i++)
            {
                Vector3 position = m_PointsPositions[i];

                position += m_Origin;

                m_Points[i].SetPosition(position);
                m_Points[i + m_VerticesAtTheBaseCount].SetPosition(position + m_Offset);
            }
        }
    }
}