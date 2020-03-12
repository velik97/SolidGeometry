using System.Collections.Generic;
using System.Linq;
using Lesson.Shapes.Datas;

namespace Lesson.Validators
{
    public class PointsNotSameValidator : Validator
    {
        private readonly IEnumerable<PointData> m_Points;

        public PointsNotSameValidator(IEnumerable<PointData> points)
        {
            m_Points = points;
        }

        protected override bool CheckIsValid()
        {
            return m_Points.OrderBy(point => point?.GetHashCode() ?? 0).Distinct().Count() == m_Points.Count();
        }

        public override string GetNotValidMessage()
        {
            return "Points should be distinct";
        }
    }
}