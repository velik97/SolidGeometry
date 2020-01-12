using System;
using System.Collections.Generic;
using System.Linq;
using Shapes.View;
using UnityEngine;

namespace Shapes.Data
{
    public class ShapeDataFactory
    {
        private readonly List<PointData> m_PointDatas = new List<PointData>();
        private readonly List<LineData> m_LinetDatas = new List<LineData>();
        private readonly List<PolygonData> m_PolygonDatas = new List<PolygonData>();
        private readonly List<CompositeShapeData> m_CompositeShapeDatas = new List<CompositeShapeData>();

        private readonly HashSet<string> m_PointsHashset = new HashSet<string>();
        private readonly HashSet<string[]> m_LinesHasSet = new HashSet<string[]>(new NonPositionalArrayEqualityComparer());
        private readonly HashSet<string[]> m_PolygonsHashSet = new HashSet<string[]>(new NonPositionalArrayEqualityComparer());

        private ShapeViewFactory m_ShapeViewFactory;

        public ShapeDataFactory(ShapeViewFactory shapeViewFactory)
        {
            m_ShapeViewFactory = shapeViewFactory;
        }

        public PointData CreatePointData(Vector3 position, string pointName, bool isAccessoryPoint)
        {
            PointData pointData = new PointData(position, pointName, isAccessoryPoint);
            pointData.AttachView(m_ShapeViewFactory.RequestPointView(pointData));
            
            m_PointDatas.Add(pointData);
            m_PointsHashset.Add(pointData.PointName);
            return pointData;
        }
        
        public PointData CreatePointData(Func<Vector3> positionFunc, string pointName, bool isAccessoryPoint)
        {
            PointData pointData = new ConditionalPointData(positionFunc, pointName, isAccessoryPoint);
            pointData.AttachView(m_ShapeViewFactory.RequestPointView(pointData));
            
            m_PointDatas.Add(pointData);
            m_PointsHashset.Add(pointData.PointName);
            return pointData;
        }

        public LineData CreateLineData(PointData startPoint, PointData endPoint)
        {
            LineData lineData = new LineData(startPoint, endPoint);
            lineData.AttachView(m_ShapeViewFactory.RequestLineView(lineData));
            
            m_LinetDatas.Add(lineData);
            m_LinesHasSet.Add(new []{startPoint.PointName, endPoint.PointName});
            return lineData;
        }

        public PolygonData CreatePolygonData(params PointData[] pointDatas)
        {
            PolygonData polygonData = new PolygonData(pointDatas);
            polygonData.AttachView(m_ShapeViewFactory.RequestPolygonView(polygonData));
            
            m_PolygonDatas.Add(polygonData);
            m_PolygonsHashSet.Add(pointDatas.Select(p => p.PointName).ToArray());
            return polygonData;
        }

        public CompositeShapeData CreateCompositeShapeData(
            PointData[] pointDatas,
            LineData[] lineDatas,
            PolygonData[] polygonDatas,
            string shapeName)
        {
            CompositeShapeData compositeShapeData = new CompositeShapeData(pointDatas, lineDatas, polygonDatas, shapeName);
            compositeShapeData.AttachView(m_ShapeViewFactory.RequestCompositeShapeView(compositeShapeData));
            
            m_CompositeShapeDatas.Add(compositeShapeData);
            return compositeShapeData;
        }
        
        public void RemoveShapeData(ShapeData data)
        {
            switch (data)
            {
                case PointData pointData:
                    m_PointDatas.Remove(pointData);
                    m_PointsHashset.Remove(pointData.PointName);
                    break;
                case LineData lineData:
                    m_LinetDatas.Remove(lineData);
                    m_LinesHasSet.Remove(new[] {lineData.StartPoint.PointName, lineData.EndPoint.PointName});
                    break;
                case PolygonData polygonData:
                    m_PolygonDatas.Remove(polygonData);
                    m_LinesHasSet.Remove(polygonData.Points.Select(p => p.PointName).ToArray());
                    break;
                case CompositeShapeData compositeShapeData:
                    m_CompositeShapeDatas.Remove(compositeShapeData);
                    break;
            }
        }

        public bool ContainsPointName(string pointName)
        {
            return m_PointsHashset.Contains(pointName);
        }

        public bool ContainsLine(string startPoint, string endPoint)
        {
            return m_LinesHasSet.Contains(new[] {startPoint, endPoint});
        }

        public bool ContainsPolygon(string[] pointNames)
        {
            return m_PolygonsHashSet.Contains(pointNames);
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
    }
}