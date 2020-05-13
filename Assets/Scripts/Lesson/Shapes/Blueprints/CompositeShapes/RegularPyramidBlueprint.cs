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
    public class RegularPyramidBlueprint : ShapeBlueprint
    {

        [JsonProperty]
        private Vector3 m_Origin;
        [JsonProperty]
        private Vector3 m_Offset;
        [JsonProperty]
        private float m_Radius;
        
        [JsonProperty]
        private int m_VerticesAtTheBaseCount = 3;
        


        [JsonProperty]
        private readonly List<PointData> m_Points = new List<PointData>();
        [JsonProperty]
        private readonly List<LineData> m_Lines = new List<LineData>();
        [JsonProperty]
        private readonly List<PolygonData> m_Polygons = new List<PolygonData>();
        
        public int VerticesAtTheBaseCount => m_VerticesAtTheBaseCount;

        public Vector3 Origin => m_Origin;
        public Vector3 Offset => m_Offset;
        public float Radius => m_Radius;


        [JsonProperty]
        private readonly CompositeShapeData m_CompositeShapeData;
        
        public IReadOnlyList<PointData> Points => m_Points;
        public IReadOnlyList<LineData> Lines => m_Lines;
        public IReadOnlyList<PolygonData> Polygons => m_Polygons;

        public override ShapeData MainShapeData => m_CompositeShapeData;

        public NonZeroVolumeValidator NonZeroVolumeValidator;

        public RegularPyramidBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            m_CompositeShapeData = dataFactory.CreateCompositeShapeData();
            ConstructPyramid();
            OnDeserialized();
        }

        [JsonConstructor]
        public RegularPyramidBlueprint(object _)
        {
        }

        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            //NonZeroVolumeValidator = new NonZeroVolumeValidator(m_Axes);
            //NonZeroVolumeValidator.Update();
            UpdatePointsPositions();

            foreach (var shapeData in
                new[] {m_CompositeShapeData}.Cast<ShapeData>()
                    .Concat(m_Points)
                    .Concat(m_Lines)
                    .Concat(m_Polygons))
            {
                MyShapeDatas.Add(shapeData);
                shapeData.SourceBlueprint = this;
            }
            
            m_CompositeShapeData.SetShapeName("Regular Pyramid");
        }

        private void ConstructPyramid()
        {
            for (int i = 0; i < m_VerticesAtTheBaseCount + 1; i++)
            {
                m_Points.Add(ShapeDataFactory.CreatePointData());
                m_Points[i].NameUpdated += OnNameUpdated;
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
                MyShapeDatas.Add(shapeData);
                shapeData.SourceBlueprint = this;
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
        
        public void SetBaseVerticesCount(int count)
        {
            if (count > 15 || count < 3)
            {
                return;
            }

            m_VerticesAtTheBaseCount = count;
            
            Clear();
            ConstructPyramid();
            
            UpdatePointsPositions();
        }

        public void SetOrigin(Vector3 origin)
        {
            m_Origin = origin;
            UpdatePointsPositions();
        }
        public void SetOffset(Vector3 offset)
        {
            m_Offset = offset;
            UpdatePointsPositions();
        }
        
        public void SetRadius(float radius)
        {
            if (radius < 0)
            {
                return;
            }

            m_Radius = radius;
            
            UpdatePointsPositions();
        }
        
        private void UpdatePointsPositions()
        {
            var side = 2 * m_Radius * Mathf.Sin(Mathf.PI / m_VerticesAtTheBaseCount);

            for (int i = 0; i < m_VerticesAtTheBaseCount; i++)
            {
                Vector3 position = new Vector3(
                    m_Radius * Mathf.Cos(2 * Mathf.PI * i / m_VerticesAtTheBaseCount),
                    0f,
                    m_Radius * Mathf.Sin(2 * Mathf.PI * i / m_VerticesAtTheBaseCount));

                position += m_Origin;
                
                m_Points[i].SetPosition(position);
            }
            m_Points[m_VerticesAtTheBaseCount].SetPosition(m_Offset);
        }
    }
}