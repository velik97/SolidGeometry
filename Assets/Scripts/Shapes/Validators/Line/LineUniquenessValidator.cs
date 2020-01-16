using System;
using Shapes.Data;
using Shapes.Validators.Uniqueness;

namespace Shapes.Validators.Line
{
    public class LineUniquenessValidator : IUniquenessValidator<LineUniquenessValidator>, IValidator
    {
        public event Action UniqueDeterminingPropertyUpdated;
        public event Action ValidStateChanged;

        private readonly LineData m_LineData;
            
        private bool m_IsUnique;
        
        public LineUniquenessValidator(LineData lineData)
        {
            m_LineData = lineData;
            m_LineData.NameUpdated += OnUniqueDeterminingPropertyUpdated;
        }
            
        private void OnUniqueDeterminingPropertyUpdated()
        {
            UniqueDeterminingPropertyUpdated?.Invoke();
        }

        public bool IsValid()
        {
            return m_IsUnique;
        }

        public string GetNotValidMessage()
        {
            return $"Line '{m_LineData.StartPoint?.PointName}{m_LineData.EndPoint?.PointName}' is not unique";
        }

        public void SetIsUnique(bool unique)
        {
            if (unique == m_IsUnique)
            {
                return;
            }
            m_IsUnique = unique;
            ValidStateChanged?.Invoke();
        }

        public int GetUniqueHashCode()
        {
            return ((m_LineData.StartPoint?.GetHashCode() ?? 0) ^ (m_LineData.EndPoint?.GetHashCode() ?? 0)) & 0xfffffff;
        }

        public bool UniqueEquals(LineUniquenessValidator validator)
        {
            return (m_LineData.StartPoint == validator.m_LineData.StartPoint &&
                   m_LineData.EndPoint == validator.m_LineData.EndPoint)
                   || 
                   (m_LineData.EndPoint == validator.m_LineData.StartPoint &&
                   m_LineData.StartPoint == validator.m_LineData.EndPoint);
        }
    }
}