using System.Linq;
using Lesson.Shapes.Datas;
using UnityEngine;
using Util;

namespace Lesson.Validators.Polygon
{
    public class PolygonPointsAreInOnePlaneValidator : Validator
    {
        private readonly PolygonData m_PolygonData;

        public PolygonPointsAreInOnePlaneValidator(PolygonData polygonData)
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
            
            Vector3[] points = new Vector3[m_PolygonData.Points.Count - 1];
            
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = m_PolygonData.Points[i + 1].Position - m_PolygonData.Points[0].Position;
            }
            
            for (int i = 2; i < points.Length; i++)
            {
                if (!points[i].CollinearWith(points[0], points[1]))
                {
                    return false;
                }
            }

            return true;
        }
        
        public override string GetNotValidMessage()
        {
            return "Points should be in the same plane";
        }
    }
}