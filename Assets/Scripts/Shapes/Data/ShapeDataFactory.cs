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

        private ShapeViewFactory m_ShapeViewFactory;

        public ShapeDataFactory(ShapeViewFactory shapeViewFactory)
        {
            m_ShapeViewFactory = shapeViewFactory;
        }

        public PointData CreatePointData()
        {
            PointData pointData = new PointData();
            pointData.AttachView(m_ShapeViewFactory.RequestPointView(pointData));
            
            m_PointDatas.Add(pointData);
            return pointData;
        }
        
        public PointData CreateConditionalPointData()
        {
            PointData pointData = new ConditionalPointData();
            pointData.AttachView(m_ShapeViewFactory.RequestPointView(pointData));
            
            m_PointDatas.Add(pointData);
            return pointData;
        }

        public LineData CreateLineData(PointData startPoint, PointData endPoint)
        {
            LineData lineData = new LineData(startPoint, endPoint);
            lineData.AttachView(m_ShapeViewFactory.RequestLineView(lineData));
            
            m_LinetDatas.Add(lineData);
            return lineData;
        }

        public PolygonData CreatePolygonData(params PointData[] pointDatas)
        {
            PolygonData polygonData = new PolygonData(pointDatas);
            polygonData.AttachView(m_ShapeViewFactory.RequestPolygonView(polygonData));
            
            m_PolygonDatas.Add(polygonData);
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