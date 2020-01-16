using System;
using Shapes.Data;
using Shapes.Validators.Uniqueness;

namespace Shapes.Validators.Point
{
    public class PointNameUniquenessValidator : IUniquenessValidator<PointNameUniquenessValidator>, IValidator
    {
        public event Action UniqueDeterminingPropertyUpdated;
        public event Action ValidStateChanged;

        private readonly PointData m_PointData;
            
        private bool m_IsUnique;

        private string PointName => m_PointData.PointName;

        public PointNameUniquenessValidator(PointData pointData)
        {
            m_PointData = pointData;
            m_PointData.NameUpdated += OnUniqueDeterminingPropertyUpdated;
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
            return $"Name '{PointName}' is already taken";
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
            return PointName.GetHashCode() & 0xfffffff;
        }

        public bool UniqueEquals(PointNameUniquenessValidator validator)
        {
            return PointName == validator.PointName;
        }
    }
}