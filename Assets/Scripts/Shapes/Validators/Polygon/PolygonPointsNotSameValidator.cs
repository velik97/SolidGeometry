using System;
using System.Linq;
using Shapes.Data;

namespace Shapes.Validators.Polygon
{
    public class PolygonPointsNotSameValidator : Validator
    {
        private readonly PolygonData m_PolygonData; 
            
        
        public PolygonPointsNotSameValidator(PolygonData polygonData)
        {
            m_PolygonData = polygonData;
            m_PolygonData.NameUpdated += UpdateValidState;
        }

        protected override bool CheckIsValid()
        {
            return m_PolygonData.Points.OrderBy(point => point?.GetHashCode() ?? 0).Distinct().Count() == m_PolygonData.Points.Count;
        }

        public override string GetNotValidMessage()
        {
            return "Points should be distinct";
        }
    }
}