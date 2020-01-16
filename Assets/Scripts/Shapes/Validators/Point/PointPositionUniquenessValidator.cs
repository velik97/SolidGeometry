using System;
using Shapes.Data;
using Shapes.Validators.Uniqueness;
using UnityEngine;

namespace Shapes.Validators.Point
{
    public class PointPositionUniquenessValidator : IUniquenessValidator<PointPositionUniquenessValidator>, IValidator
    {
        public event Action UniqueDeterminingPropertyUpdated;
        public event Action ValidStateChanged;

        private readonly PointData m_PointData;
            
        private bool m_IsUnique;

        public bool IsUnique => m_IsUnique;

        public Vector3 Position => m_PointData.Position;

        public bool IsValid()
        {
            return m_IsUnique;
        }

        public string GetNotValidMessage()
        {
            return $"Position '{Position}' is already taken";
        }

        public PointPositionUniquenessValidator(PointData pointData)
        {
            m_PointData = pointData;
            m_PointData.GeometryUpdated += OnUniqueDeterminingPropertyUpdated;
        }
            
        private void OnUniqueDeterminingPropertyUpdated()
        {
            UniqueDeterminingPropertyUpdated?.Invoke();
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
            return Position.GetHashCode() & 0xfffffff;
        }

        public bool UniqueEquals(PointPositionUniquenessValidator validator)
        {
            return Position == validator.Position;
        }
    }
}