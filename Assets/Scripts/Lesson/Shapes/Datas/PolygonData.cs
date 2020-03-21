using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Lesson.Shapes.Views;
using Lesson.Validators;
using Lesson.Validators.Polygon;
using Newtonsoft.Json;
using Serialization;
using UnityEngine;

namespace Lesson.Shapes.Datas
{
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class PolygonData : ShapeData
    {
        public PolygonView PolygonView => View as PolygonView;

        public IReadOnlyList<PointData> Points => m_Points;
        
        [JsonProperty]
        private readonly List<PointData> m_Points = new List<PointData>();
        
        public bool CanRemovePoints => m_Points.Count > 3;

        public PolygonPointsAreInOnePlaneValidator PointsAreInOnePlaneValidator;
        public PolygonPointsAreOnSameLineValidator PointsAreOnSameLineValidator;
        public PointsNotSameValidator PointsNotSameValidator;
        public PolygonLinesDontIntersectValidator LinesDontIntersectValidator;

        public PolygonUniquenessValidator PolygonUniquenessValidator;

        public PolygonData()
        {
            // Polygon should have at least three points
            m_Points.Add(null);
            m_Points.Add(null);
            m_Points.Add(null);

            OnDeserialized();
        }
        
        [JsonConstructor]
        public PolygonData(JsonConstructorMark _)
        { }
        
        [OnDeserialized, UsedImplicitly]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (PointData pointData in m_Points)
            {
                if (pointData != null)
                {
                    SubscribeOnPoint(pointData);
                }
            }
            OnDeserialized();
        }
        
        private void OnDeserialized()
        {
            PointsAreInOnePlaneValidator = new PolygonPointsAreInOnePlaneValidator(this);
            PointsAreOnSameLineValidator = new PolygonPointsAreOnSameLineValidator(this);
            LinesDontIntersectValidator = new PolygonLinesDontIntersectValidator(this);
            PolygonUniquenessValidator = new PolygonUniquenessValidator(this);
            
            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            NameUpdated += PointsNotSameValidator.Update;
            
            PointsAreInOnePlaneValidator.Update();
            PointsAreOnSameLineValidator.Update();
            LinesDontIntersectValidator.Update();
            PointsNotSameValidator.Update();
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            foreach (PointData pointData in Points)
            {
                yield return pointData;
            }
        }

        public void SetPointsCount(int count)
        {
            if (count < 3)
            {
                Debug.LogError("Polygon have to have at least 3 points");
                return;
            }

            while (m_Points.Count < count)
            {
                AddPoint();
            }

            while (m_Points.Count > count)
            {
                RemovePoint(m_Points.Count - 1);
            }
        }

        public void AddPoint()
        {
            m_Points.Add(null);
        }

        public void RemovePoint(int index)
        {
            if (m_Points.Count <= 3)
            {
                // Polygon should have at least three points
                return;
            }

            if (m_Points.Count <= index)
            {
                Debug.LogError("Index out of array");
                return;
            }
            
            UnsubscribeFromPoint(m_Points[index]);
            m_Points.RemoveAt(index);
            OnGeometryUpdated();
        }

        public void SetPoint(int index, PointData pointData)
        {
            if (m_Points.Count <= index)
            {
                Debug.LogError("Index out of array");
                return;
            }

            UnsubscribeFromPoint(m_Points[index]);
            m_Points[index] = pointData;
            SubscribeOnPoint(m_Points[index]);
        }

        public override string ToString()
        {
            return $"Polygon {string.Join("", m_Points.Select(p => p?.PointName))}";
        }
    }
}