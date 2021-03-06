using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Datas;
using Lesson.Validators.VolumeShape;
using Newtonsoft.Json;
using UnityEngine;

namespace Lesson.Shapes.Blueprints.CompositeShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PyramidBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        private int m_VerticesAtTheBaseCount = 3;
        [JsonProperty]
        private Vector3 m_Origin;
        [JsonProperty]
        private Vector3 m_TopVertex;
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
        public Vector3 TopVertex => m_TopVertex;
        public IReadOnlyList<Vector3> PointsPositions => m_PointsPositions;

        public IReadOnlyList<PointData> Points => m_Points;
        public IReadOnlyList<LineData> Lines => m_Lines;
        public IReadOnlyList<PolygonData> Polygons => m_Polygons;

        public override ShapeData MainShapeData => m_CompositeShapeData;

        public NonZeroVolumeValidator NonZeroVolumeValidator;

        public PyramidBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            m_CompositeShapeData = dataFactory.CreateCompositeShapeData();
            ConstructPyramid();
            OnDeserialized();
        }

        [JsonConstructor]
        public PyramidBlueprint(object _)
        {
        }

        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var shapeData in
                new[] {m_CompositeShapeData}.Cast<ShapeData>()
                    .Concat(m_Points)
                    .Concat(m_Lines)
                    .Concat(m_Polygons))
            {
                AddToMyShapeDatas(shapeData);
            }
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            //NonZeroVolumeValidator = new NonZeroVolumeValidator(m_Axes);
            //NonZeroVolumeValidator.Update();
            GeometryUpdated.Invoke();
            m_CompositeShapeData.SetShapeName("Pyramid");
        }

        
        private void ConstructPyramid()
        {
            UpdateVerticesPositionsList();
            for (int i = 0; i < m_VerticesAtTheBaseCount + 1; i++)
            {
                m_Points.Add(ShapeDataFactory.CreatePointData());
                m_Points[i].NameUpdated.Subscribe(NameUpdated);
            }

            for (int i = 0; i < 2 * m_VerticesAtTheBaseCount; i++)
            {
                m_Lines.Add(ShapeDataFactory.CreateLineData());
            }

            for (int i = 0; i < m_VerticesAtTheBaseCount + 1; i++)
            {
                m_Polygons.Add(ShapeDataFactory.CreatePolygonData());
                // Side faces
                if (i < m_VerticesAtTheBaseCount)
                {
                    m_Polygons[i].SetPointsCount(3);
                }
                // Bottom face
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
                AddToMyShapeDatas(shapeData);
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

            // Side edges
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Lines[i + m_VerticesAtTheBaseCount].SetStartPoint(m_Points[i]);
                m_Lines[i + m_VerticesAtTheBaseCount].SetEndPoint(m_Points[m_VerticesAtTheBaseCount]);
            }
        }

        private void ConstructPolygons()
        {
            // Bottom face
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Polygons[m_VerticesAtTheBaseCount].SetPoint(i, m_Points[i]);
            }

            // Side faces
            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                m_Polygons[i].SetPoint(0, m_Points[i]);
                m_Polygons[i].SetPoint(1, m_Points[(i + 1) % m_VerticesAtTheBaseCount]);
                m_Polygons[i].SetPoint(2, m_Points[m_VerticesAtTheBaseCount]);
            }
        }

        private void Clear()
        {
            ClearMyShapeDatas();

            foreach (PointData pointData in m_Points)
            {
                pointData.NameUpdated.Unsubscribe(NameUpdated);
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
        
        public void SetBaseVerticesCount(int count)
        {
            if (count > 15 || count < 3)
            {
                return;
            }

            m_VerticesAtTheBaseCount = count;
            
            Clear();
            ConstructPyramid();
            
            GeometryUpdated.Invoke();
        }
        public void SetOffset(Vector3 offset)
        {
            m_TopVertex = offset;
            GeometryUpdated.Invoke();
        }

        public void SetOrigin(Vector3 origin)
        {
            if (m_Origin == origin)
            {
                return;
            }

            m_Origin = origin;
            GeometryUpdated.Invoke();
        }

        public void SetPointPosition(int pointIndex, Vector3 position)
        {
            if (pointIndex < 0 || pointIndex >= m_PointsPositions.Count)
            {
                return;
            }

            m_PointsPositions[pointIndex] = position;
            GeometryUpdated.Invoke();
        }

        protected override void UpdateGeometry()
        {
            for (int i = 0; i < m_PointsPositions.Count; i++)
            {
                Vector3 position = m_PointsPositions[i];

                position += m_Origin;

                m_Points[i].SetPosition(position);
            }
            m_Points[m_VerticesAtTheBaseCount].SetPosition(m_TopVertex);

        }
    }
}