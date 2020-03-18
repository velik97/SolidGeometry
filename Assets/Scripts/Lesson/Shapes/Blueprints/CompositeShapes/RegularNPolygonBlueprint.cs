using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Data;
using Lesson.Shapes.Datas;
using Lesson.Validators.VolumeShape;
using Newtonsoft.Json;
using Shapes.Blueprint;
using Shapes.Data;
using UnityEngine;

namespace Lesson.Shapes.Blueprints.CompositeShapes
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class RegularNPolygonBlueprint: ShapeBlueprint
    {
        [JsonProperty] private Vector3 m_Origin;
        [JsonProperty] private float m_Radius = 3;
        [JsonProperty] private float m_Height;
        [JsonProperty] private int m_N = 3;

        [JsonProperty] private readonly CompositeShapeData m_CompositeShapeData;
        
        [JsonProperty] private readonly List<PointData> m_Points = new List<PointData>();
        [JsonProperty] private readonly List<LineData> m_Lines = new List<LineData>();
        [JsonProperty] private readonly List<PolygonData> m_Polygons = new List<PolygonData>();
        
        public Vector3 Origin => m_Origin;
        public float Radius => m_Radius;
        public float Height => m_Height;
        public int N => m_N;

        public IReadOnlyList<PointData> Points => m_Points;
        public IReadOnlyList<LineData> Lines => m_Lines;
        public IReadOnlyList<PolygonData> Polygons => m_Polygons;

        public override ShapeData MainShapeData => m_CompositeShapeData;

        public NonZeroVolumeValidator NonZeroVolumeValidator;

        public RegularNPolygonBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
        {
            
                for (int i = 0; i < 2*m_N; i++)
                {
                    m_Points.Add(dataFactory.CreatePointData()) ;
                    m_Points[i].NameUpdated += OnNameUpdated;
                }

                for (int i = 0; i < 3*m_N; i++)
                {
                    m_Lines.Add(dataFactory.CreateLineData());
                }

                for (int i = 0; i < m_N + 2; i++)
                {
                    m_Polygons.Add(dataFactory.CreatePolygonData());
                    if(i < m_N)
                        m_Polygons[i].AddPoint();
                }

                ConstructLines();
                ConstructPolygons();

                m_CompositeShapeData = dataFactory.CreateCompositeShapeData();

                m_CompositeShapeData.SetShapeName("RegularNPolygon");
                m_CompositeShapeData.SetPoints(m_Points.ToArray());
                m_CompositeShapeData.SetLines(m_Lines.ToArray());
                m_CompositeShapeData.SetPolygons(m_Polygons.ToArray());

                OnDeserialized();
            
            
        }

        [JsonConstructor]
        public RegularNPolygonBlueprint(object _)
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
        }

        private void ConstructLines()
        {
            // bot
            for (int i = 0; i < m_N; i++)
            {
                m_Lines[i].SetStartPoint(m_Points[i % m_N]);
                m_Lines[i].SetEndPoint(m_Points[(i + 1) % m_N]);
            }
            
            // top
            for (int i = 0; i < m_N; i++)
            {
                m_Lines[i + m_N].SetStartPoint(m_Points[(i % m_N) + m_N]);
                m_Lines[i + m_N].SetEndPoint(m_Points[((i + 1) % m_N) + m_N]);
            }
            
            // side edges
            for (int i = 0; i < m_N; i++)
            {
                m_Lines[i + 2*m_N].SetStartPoint(m_Points[i]);
                m_Lines[i + 2*m_N].SetEndPoint(m_Points[i+m_N]);
            }
        }

        private void ConstructPolygons()
        {
            
            // Top face
            m_Polygons[m_N + 1].SetPoint(0, m_Points[0 + m_N]);
            m_Polygons[m_N + 1].SetPoint(1, m_Points[1 + m_N]);
            m_Polygons[m_N + 1].SetPoint(2, m_Points[2 + m_N]);
            
            // Bottom face
            m_Polygons[m_N].SetPoint(0, m_Points[0]);
            m_Polygons[m_N].SetPoint(1, m_Points[1]);
            m_Polygons[m_N].SetPoint(2, m_Points[2]);

            // Side faces
            for (int i = 0; i < m_N; i++)
            {
                m_Polygons[i].SetPoint(0, m_Points[i]);
                m_Polygons[i].SetPoint(3, m_Points[(i + 1) % m_N]);
                m_Polygons[i].SetPoint(1, m_Points[m_N + i]);
                m_Polygons[i].SetPoint(2, m_Points[((i + 1) % m_N) + m_N]);
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

        public void SetRadius(float radius)
        {
            if (radius < 0)
            {
                return;
            }

            m_Radius = radius;
            //NonZeroVolumeValidator.Update();
            UpdatePointsPositions();
        }
        
        public void SetN(int n)
        {
            if (n > 15 || n < 3)
                return;
            if (n > m_N)
            {
                for (int i = 2 * m_N; i < 2 * n; i++)
                {
                    m_Points.Add(ShapeDataFactory.CreatePointData());
                    m_Points[i].NameUpdated += OnNameUpdated;

                }

                for (int i = 3 * m_N; i < 3 * n; i++)
                {
                    m_Lines.Add(ShapeDataFactory.CreateLineData());
                }

                for (int i = m_N + 2; i < n + 2; i++)
                {
                    m_Polygons.Add(ShapeDataFactory.CreatePolygonData());
                }
            }
            else
            {
                /*foreach (var pointData in ShapeDataFactory.PointDatas)
                {
                    ShapeDataFactory.RemoveShapeData(pointData);
                }
                
                for (int i = 0; i < 2*n; i++)
                {
                    m_Points.Add(ShapeDataFactory.CreatePointData()) ;
//                    m_Points[i].NameUpdated += OnNameUpdated;

                }*/
            }
            m_N = n;

            ConstructLines();
            ConstructPolygons();
            UpdatePointsPositions();

            //NonZeroVolumeValidator.Update();

        }

        public void SetHeight(float height)
        {
            if (height < 0)
            {
                return;
            }

            m_Height = height;
            UpdatePointsPositions();
        }

        private void UpdatePointsPositions()
        {
            var H = new Vector3(0, 0, m_Height);
            for (int i = 0; i < m_N; i++)
            {
                Vector3 v = new Vector3(m_Origin.x + m_Radius*Mathf.Cos(2*Mathf.PI*i/m_N),m_Origin.y + m_Radius*Mathf.Sin(2*Mathf.PI*i/m_N), m_Origin.z);
                m_Points[i].SetPosition(v);
                m_Points[i+m_N].SetPosition(v + H);

            }

        }
    }
}