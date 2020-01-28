using System;
using System.Collections.Generic;
using System.Linq;
using Shapes.Validators;
using Shapes.Validators.Polygon;
using Shapes.View;
using UnityEngine;

namespace Shapes.Data
{
    [Serializable]
    public class PolygonData : ShapeData
    {
        public PolygonView PolygonView => View as PolygonView;

        public IReadOnlyList<PointData> Points => m_Points;
        
        private readonly List<PointData> m_Points = new List<PointData>();
        
        public bool CanRemovePoints => m_Points.Count > 3;

        public readonly PolygonPointsAreInOnePlaneValidator PointsAreInOnePlaneValidator;
        public readonly PolygonPointsAreOnSameLineValidator PointsAreOnSameLineValidator;
        public readonly PointsNotSameValidator PointsNotSameValidator;
        public readonly PolygonLinesDontIntersectValidator LinesDontIntersectValidator;

        public readonly PolygonUniquenessValidator PolygonUniquenessValidator;

        public PolygonData()
        {
            // Polygon should have at least three points
            m_Points.Add(null);
            m_Points.Add(null);
            m_Points.Add(null);

            PointsAreInOnePlaneValidator = new PolygonPointsAreInOnePlaneValidator(this);
            PointsAreOnSameLineValidator = new PolygonPointsAreOnSameLineValidator(this);
            LinesDontIntersectValidator = new PolygonLinesDontIntersectValidator(this);
            PolygonUniquenessValidator = new PolygonUniquenessValidator(this);
            
            PointsNotSameValidator = new PointsNotSameValidator(EnumeratePoints());
            NameUpdated += PointsNotSameValidator.Update;
        }

        private IEnumerable<PointData> EnumeratePoints()
        {
            foreach (PointData pointData in Points)
            {
                yield return pointData;
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