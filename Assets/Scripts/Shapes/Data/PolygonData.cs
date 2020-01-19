using System;
using System.Collections.Generic;
using System.Linq;
using Shapes.View;

namespace Shapes.Data
{
    [Serializable]
    public class PolygonData : ShapeData
    {
        public PolygonView PolygonView => View as PolygonView;

        public IReadOnlyList<PointData> Points => m_Points;
        
        private List<PointData> m_Points;

        public PolygonData()
        {
            // Polygon should have at least three points
            m_Points.Add(null);
            m_Points.Add(null);
            m_Points.Add(null);
        }

        public void AddPoint()
        {
            m_Points.Add(null);
        }

        public void RemoveLastPoint()
        {
            if (m_Points.Count <= 3)
            {
                // Polygon should have at least three points
                return;
            }

            m_Points.RemoveAt(m_Points.Count - 1);
        }

        public void SetPoint(int index, PointData pointData)
        {
            // Надо настроить подписки
            жфвла вдафвраы 
            if (m_Points.Count <= index)
            {
                return;
            }

            m_Points[index] = pointData;
        }

        public override string ToString()
        {
            return $"Polygon {string.Join("", m_Points.Select(p => p.PointName))}";
        }
    }
}