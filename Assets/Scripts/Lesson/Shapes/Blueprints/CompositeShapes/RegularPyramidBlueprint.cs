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
    public class RegularPyramidBlueprint : ShapeBlueprint
    {

        [JsonProperty]
        private Vector3 m_Origin;
        [JsonProperty] 
        private float m_Length;
        [JsonProperty] 
        private float m_Height;

        [JsonProperty] private readonly PointData[] m_Points = new PointData[4];
        [JsonProperty] private readonly LineData[] m_Lines = new LineData[6];
        [JsonProperty] private readonly PolygonData[] m_Polygons = new PolygonData[4];
        [JsonProperty] private readonly CompositeShapeData m_CompositeShapeData;

        public Vector3 Origin => m_Origin;
        public float Length => m_Length;
        
        public float Height => m_Height;


        public IReadOnlyList<PointData> Points => m_Points;
        public IReadOnlyList<LineData> Lines => m_Lines;
        public IReadOnlyList<PolygonData> Polygons => m_Polygons;

        public override ShapeData MainShapeData => m_CompositeShapeData;

        public NonZeroVolumeValidator NonZeroVolumeValidator;

        public RegularPyramidBlueprint(ShapeDataFactory dataFactory) : base(dataFactory)
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
            }

            ConstructLines();
            ConstructPolygons();

            m_CompositeShapeData = dataFactory.CreateCompositeShapeData();

            m_CompositeShapeData.SetShapeName("RegularPyramid");
            m_CompositeShapeData.SetPoints(m_Points);
            m_CompositeShapeData.SetLines(m_Lines);
            m_CompositeShapeData.SetPolygons(m_Polygons);

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
        }

        private void ConstructLines()
        {
            // Bottom edges
            for (int i = 0; i < 3; i++)
            {
                m_Lines[i].SetStartPoint(m_Points[i % 3]);
                m_Lines[i].SetEndPoint(m_Points[(i + 1) % 3]);
            }

            // Side edges
            for (int i = 0; i < 3; i++)
            {
                m_Lines[i + 3].SetStartPoint(m_Points[i]);
                m_Lines[i + 3].SetEndPoint(m_Points[3]);
            }
        }

        private void ConstructPolygons()
        {
            // Bottom face
            for (int i = 0; i < 3; i++)
            {
                m_Polygons[0].SetPoint(i, m_Points[i]);
            }


            // Side faces
            for (int i = 0; i < 2; i++)
            {
                m_Polygons[1].SetPoint(i, m_Points[i]);
                m_Polygons[2].SetPoint(i, m_Points[i + 1]);
            }

            m_Polygons[1].SetPoint(2, m_Points[3]);
            m_Polygons[2].SetPoint(2, m_Points[3]);

            m_Polygons[3].SetPoint(0, m_Points[0]);
            m_Polygons[3].SetPoint(1, m_Points[2]);
            m_Polygons[3].SetPoint(2, m_Points[3]);



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

        public void SetLength(float length)
        {
            if (length < 0 )
            {
                return;
            }

            m_Length = length;
            //NonZeroVolumeValidator.Update();
            UpdatePointsPositions();
        }
        public void SetHeight(float height)
        {
            if (height < 0 )
            {
                return;
            }

            m_Height = height;
            UpdatePointsPositions();
        }
        private void UpdatePointsPositions()
        {
            var v1 = new Vector3(m_Length, 0, 0);
            var v2 = new Vector3(m_Length* 1/2,m_Length*Mathf.Sqrt(3)/2, 0);
            var height = new Vector3(0, 0, m_Height);
            
            
            m_Points[0].SetPosition(m_Origin);
            m_Points[1].SetPosition(m_Origin + v1);
            m_Points[2].SetPosition(m_Origin + v2);
            m_Points[3].SetPosition(m_Origin + (v1 + v2)/3 + height); //hope my maths true

        }
    }
}