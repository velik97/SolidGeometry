using System.Linq;
using Lesson.Shapes.Datas;
using UnityEngine;
using Util;

namespace Lesson.Validators.Polygon
{
    public class PolygonLinesDontIntersectValidator : Validator
    {
        private readonly PolygonData m_PolygonData;

        public PolygonLinesDontIntersectValidator(PolygonData polygonData)
        {
            m_PolygonData = polygonData;
            m_PolygonData.GeometryUpdated.Subscribe(base.Update);
        }

        protected override bool CheckIsValid()
        {
            if (m_PolygonData.Points.Count <= 3)
            {
                return true;
            }
            
            if (m_PolygonData.Points.Any(point => point == null))
            {
                return true;
            }

            Vector3 GetPosition(int index)
            {
                return m_PolygonData.Points[index].Position;
            }
            
            for (int i = 0; i < m_PolygonData.Points.Count - 2; i++)
            {
                for (int j = i + 2; j < m_PolygonData.Points.Count - 1; j++)
                {
                    if (GeometryUtils.LineSegmentsIntersects(
                        GetPosition(i), GetPosition(i + 1),
                        GetPosition(j), GetPosition(j + 1)))
                    {
                        return false;
                    }
                }
            }

            int first = 0;
            int last = m_PolygonData.Points.Count - 1;

            for (int i = 1; i < m_PolygonData.Points.Count - 2; i++)
            {
                if (GeometryUtils.LineSegmentsIntersects(
                    GetPosition(last), GetPosition(first),
                    GetPosition(i), GetPosition(i + 1)))
                {
                    return false;
                }
            }

            return true;
        }
        

        public override string GetNotValidMessage()
        {
            return "No two edges should intersect";
        }
    }
}