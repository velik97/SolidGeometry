using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Shapes.Data;
using Shapes.Validators.Parallelepiped;
using UnityEngine;

namespace Shapes.Blueprint.Figures
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class ParallelepipedBlueprint : ShapeBlueprint
    {
        [JsonProperty]
        private Vector3 m_Origin;

        [JsonProperty]
        private readonly Vector3[] m_Axes = new Vector3[3];

        [JsonProperty]
        private readonly PointData[] m_Points = new PointData[8];
        [JsonProperty]
        private readonly LineData[] m_Lines = new LineData[12];
        [JsonProperty]
        private readonly PolygonData[] m_Polygons = new PolygonData[6];
        [JsonProperty]
        private readonly CompositeShapeData m_CompositeShapeData;

        public Vector3 Origin => m_Origin;
        public IReadOnlyList<Vector3> Axes => m_Axes;
        
        public IReadOnlyList<PointData> Points => m_Points;
        public IReadOnlyList<LineData> Lines => m_Lines;
        public IReadOnlyList<PolygonData> Polygons => m_Polygons;
        
        public override ShapeData MainShapeData => m_CompositeShapeData;

        public NonZeroVolumeValidator NonZeroVolumeValidator;

        public ParallelepipedBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            for (int i = 0; i < m_Points.Length; i++)
            {
                m_Points[i] = dataFactory.CreatePointData();
                m_Points[i].NameUpdated += OnNameUpdated;
            }
            for (int i = 0; i < m_Lines.Length; i++)
            {
                m_Lines[i] = dataFactory.CreateLineData();
            }
            for (int i = 0; i < m_Polygons.Length; i++)
            {
                m_Polygons[i] = dataFactory.CreatePolygonData();
                m_Polygons[i].AddPoint(); // By default polygon has 3 points, we need 4
            }

            ConstructLines();
            ConstructPolygons();

            m_CompositeShapeData = dataFactory.CreateCompositeShapeData();
            
            m_CompositeShapeData.SetShapeName("Parallelepiped");
            m_CompositeShapeData.SetPoints(m_Points);
            m_CompositeShapeData.SetLines(m_Lines);
            m_CompositeShapeData.SetPolygons(m_Polygons);
            
            OnDeserialized();
        }
        
        [JsonConstructor]
        public ParallelepipedBlueprint(object _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            OnDeserialized();
        }

        private void OnDeserialized()
        {
            NonZeroVolumeValidator = new NonZeroVolumeValidator(m_Axes);
            NonZeroVolumeValidator.Update();
            UpdatePointsPositions();

            foreach (var shapeData in 
                new [] {m_CompositeShapeData}.Cast<ShapeData>()
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
            for (int i = 0; i < 4; i++)
            {
                m_Lines[i].SetStartPoint(m_Points[i % 4]);
                m_Lines[i].SetEndPoint(m_Points[(i + 1) % 4]);
            }

            // Side edges
            for (int i = 0; i < 4; i++)
            {
                m_Lines[i + 4].SetStartPoint(m_Points[i]);
                m_Lines[i + 4].SetEndPoint(m_Points[i + 4]);
            }
            
            // Top edges
            for (int i = 0; i < 4; i++)
            {
                m_Lines[i + 8].SetStartPoint(m_Points[4 + i % 4]);
                m_Lines[i + 8].SetEndPoint(m_Points[4 + (i + 1) % 4]);
            }
        }

        private void ConstructPolygons()
        { 
            // Bottom face
            for (int i = 0; i < 4; i++)
            {
                m_Polygons[0].SetPoint(i, m_Points[i]);
            }

            // Side faces
            for (int i = 0; i < 4; i++)
            {
                m_Polygons[i + 1].SetPoint(0, m_Points[i % 4]);
                m_Polygons[i + 1].SetPoint(1, m_Points[(i + 1) % 4]);
                m_Polygons[i + 1].SetPoint(2, m_Points[4 + (i + 1) % 4]);
                m_Polygons[i + 1].SetPoint(3, m_Points[4 + i % 4]);
            }
            
            // Top face
            for (int i = 0; i < 4; i++)
            {
                m_Polygons[5].SetPoint(i, m_Points[4 + i]);
            }
        }

        public void SetPointName(int pointIndex, string name)
        {
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

        public void SetAxis(int axisNum, Vector3 axis)
        {
            if (axisNum < 0 || axisNum >= 3)
            {
                return;
            }

            m_Axes[axisNum] = axis;
            NonZeroVolumeValidator.Update();
            UpdatePointsPositions();
        }

        private void UpdatePointsPositions()
        {
            m_Points[0].SetPosition(m_Origin);
            m_Points[1].SetPosition(m_Origin + m_Axes[0]);
            m_Points[2].SetPosition(m_Origin + m_Axes[0] + m_Axes[1]);
            m_Points[3].SetPosition(m_Origin + m_Axes[1]);
            
            m_Points[4].SetPosition(m_Origin + m_Axes[2]);
            m_Points[5].SetPosition(m_Origin + m_Axes[0] + m_Axes[2]);
            m_Points[6].SetPosition(m_Origin + m_Axes[0] + m_Axes[1] + m_Axes[2]);
            m_Points[7].SetPosition(m_Origin + m_Axes[1] + m_Axes[2]);
        }
    }
}