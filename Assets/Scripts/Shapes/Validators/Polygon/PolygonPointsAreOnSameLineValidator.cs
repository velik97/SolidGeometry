using System.Linq;
using Shapes.Data;
using UnityEngine;

namespace Shapes.Validators.Polygon
{
    public class PolygonPointsAreOnSameLineValidator : Validator
    {
        private readonly PolygonData m_PolygonData;

        public PolygonPointsAreOnSameLineValidator(PolygonData polygonData)
        {
            m_PolygonData = polygonData;
            m_PolygonData.GeometryUpdated += UpdateValidState;
        }
        
        protected override bool CheckIsValid()
        {
            if (m_PolygonData.Points.Count <= 2)
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
            
            for (var i = 0; i < m_PolygonData.Points.Count; i++)
            {
                int first = i;
                int second = (i + 1) % m_PolygonData.Points.Count;
                int third = (i + 2) % m_PolygonData.Points.Count;

                if (Vector3.Cross(
                        GetPosition(second) - GetPosition(first), 
                        GetPosition(third) - GetPosition(first))
                    == Vector3.zero)
                {
                    return false;
                }
            }

            return true;
        }

        public override string GetNotValidMessage()
        {
            return "No any three sequential points should be on same line";
        }
    }
}