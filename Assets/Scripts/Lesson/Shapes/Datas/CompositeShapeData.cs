using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Views;
using Newtonsoft.Json;
using Serialization;

namespace Lesson.Shapes.Datas
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class CompositeShapeData : ShapeData
    {
        CompositeShapeView CompositeShapeView => View as CompositeShapeView;
        
        [JsonProperty]
        private PointData[] m_Points;
        [JsonProperty]
        private LineData[] m_Lines;
        [JsonProperty]
        private PolygonData[] m_Polygons;

        public PointData[] Points => m_Points;
        public LineData[] Lines => m_Lines;
        public PolygonData[] Polygons => m_Polygons;

        private string m_ShapeName;

        public CompositeShapeData()
        {
        }
        
        [JsonConstructor]
        public CompositeShapeData(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            if (m_Points != null && m_Points.Length > 0)
            {
                foreach (PointData pointData in m_Points)
                {
                    SubscribeOnPoint(pointData);
                }
            }
            OnDeserialized();
        }
        
        private void OnDeserialized()
        { }

        public void SetShapeName(string shapeName)
        {
            m_ShapeName = shapeName;
            OnNameUpdated();
        }

        public void SetPoints(PointData[] points)
        {
            if (m_Points != null && m_Points.Length > 0)
            {
                foreach (PointData pointData in m_Points)
                {
                    UnsubscribeFromPoint(pointData);
                }
            }
            m_Points = points;
            OnNameUpdated();
            if (m_Points != null && m_Points.Length > 0)
            {
                foreach (PointData pointData in m_Points)
                {
                    SubscribeOnPoint(pointData);
                }
            }
        }

        public void SetLines(LineData[] lines)
        {
            m_Lines = lines;
        }

        public void SetPolygons(PolygonData[] polygons)
        {
            m_Polygons = polygons;
        }

        public override string ToString()
        {
            return (m_ShapeName ?? "") + " " +
                   (m_Points != null ? string.Join("", m_Points.Select(p => p?.PointName)) : "");
        }
    }
}