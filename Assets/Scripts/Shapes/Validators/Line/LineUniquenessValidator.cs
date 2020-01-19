using System;
using Shapes.Data;
using Shapes.Validators.Uniqueness;

namespace Shapes.Validators.Line
{
    public class LineUniquenessValidator : UniquenessValidator<LineUniquenessValidator>
    {
        private readonly LineData m_LineData;
        
        public LineUniquenessValidator(LineData lineData)
        {
            m_LineData = lineData;
            m_LineData.NameUpdated += OnUniqueDeterminingPropertyUpdated;
        }

        public override string GetNotValidMessage()
        {
            return $"{m_LineData} is not unique";
        }

        public override int GetUniqueHashCode()
        {
            return (m_LineData.StartPoint?.GetHashCode() ?? 0) ^ (m_LineData.EndPoint?.GetHashCode() ?? 0);
        }

        public override bool UniqueEquals(LineUniquenessValidator validator)
        {
            return (m_LineData.StartPoint == validator.m_LineData.StartPoint &&
                   m_LineData.EndPoint == validator.m_LineData.EndPoint)
                   || 
                   (m_LineData.EndPoint == validator.m_LineData.StartPoint &&
                   m_LineData.StartPoint == validator.m_LineData.EndPoint);
        }
    }
}