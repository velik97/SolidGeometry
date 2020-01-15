using System;
using System.Collections.Generic;
using System.Linq;
using Shapes.View;
using UnityEngine;

namespace Shapes.Data
{
    [Serializable]
    public class ShapeDataFactory
    {
        [SerializeField] private readonly List<PointData> m_PointDatas = new List<PointData>();
        [SerializeField] private readonly List<LineData> m_LinetDatas = new List<LineData>();
        [SerializeField] private readonly List<PolygonData> m_PolygonDatas = new List<PolygonData>();
        [SerializeField] private readonly List<CompositeShapeData> m_CompositeShapeDatas = new List<CompositeShapeData>();

        public event Action ShapesListUpdated;

        public IReadOnlyCollection<PointData> PointDatas => m_PointDatas;

        public IReadOnlyCollection<ShapeData> AllDatas =>
            m_PointDatas.Cast<ShapeData>()
                .Concat(m_LinetDatas)
                .Concat(m_PolygonDatas)
                .Concat(m_CompositeShapeDatas)
                .ToList();

        public PointData CreatePointData()
        {
            PointData pointData = new PointData();
            
            m_PointDatas.Add(pointData);
            ShapesListUpdated?.Invoke();
            pointData.NameUpdated += OnPointsListUpdated;
            return pointData;
        }
        
        public PointData CreateConditionalPointData()
        {
            PointData pointData = new ConditionalPointData();
            
            m_PointDatas.Add(pointData);
            ShapesListUpdated?.Invoke();
            pointData.NameUpdated += OnPointsListUpdated;
            return pointData;
        }

        public LineData CreateLineData()
        {
            LineData lineData = new LineData();
            
            m_LinetDatas.Add(lineData);
            ShapesListUpdated?.Invoke();
            lineData.NameUpdated += OnPointsListUpdated;
            return lineData;
        }

        public PolygonData CreatePolygonData()
        {
            PolygonData polygonData = new PolygonData();
            
            m_PolygonDatas.Add(polygonData);
            ShapesListUpdated?.Invoke();
            polygonData.NameUpdated += OnPointsListUpdated;
            return polygonData;
        }

        public CompositeShapeData CreateCompositeShapeData(
            PointData[] pointDatas,
            LineData[] lineDatas,
            PolygonData[] polygonDatas,
            string shapeName)
        {
            CompositeShapeData compositeShapeData = new CompositeShapeData(pointDatas, lineDatas, polygonDatas, shapeName);
            
            m_CompositeShapeDatas.Add(compositeShapeData);
            ShapesListUpdated?.Invoke();
            compositeShapeData.NameUpdated += OnPointsListUpdated;
            return compositeShapeData;
        }
        
        public void RemoveShapeData(ShapeData data)
        {
            data.NameUpdated -= OnPointsListUpdated;
            switch (data)
            {
                case PointData pointData:
                    m_PointDatas.Remove(pointData);
                    break;
                case LineData lineData:
                    m_LinetDatas.Remove(lineData);
                    break;
                case PolygonData polygonData:
                    m_PolygonDatas.Remove(polygonData);
                    break;
                case CompositeShapeData compositeShapeData:
                    m_CompositeShapeDatas.Remove(compositeShapeData);
                    break;
            }
            ShapesListUpdated?.Invoke();
        }
        
        private class NonPositionalArrayEqualityComparer : IEqualityComparer<string[]>
        {
            public bool Equals(string[] x, string[] y)
            {
                if (x == null && y == null)
                {
                    return true;
                }
                if (x == null ^ y == null)
                {
                    return false;
                }
                if (x == y)
                {
                    return true;
                }
                return GetHashCode(x) == GetHashCode(y);
            }

            public int GetHashCode(string[] obj)
            {
                int hash = 0;
                foreach (string s in obj)
                {
                    hash ^= s.GetHashCode();
                }

                return hash;
            }
        }

        private void OnPointsListUpdated()
        {
            ShapesListUpdated?.Invoke();
        }
    }
}