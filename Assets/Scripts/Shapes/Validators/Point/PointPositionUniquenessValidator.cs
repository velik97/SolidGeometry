using System;
using Shapes.Data;
using Shapes.Validators.Uniqueness;
using UnityEngine;

namespace Shapes.Validators.Point
{
    public class PointPositionUniquenessValidator : UniquenessValidator<PointPositionUniquenessValidator>
    {
        private readonly PointData m_PointData;

        public PointPositionUniquenessValidator(PointData pointData)
        {
            m_PointData = pointData;
            m_PointData.GeometryUpdated += OnUniqueDeterminingPropertyUpdated;
        }
        
        public override string GetNotValidMessage()
        {
            return $"Position '{m_PointData.Position}' is already taken";
        }

        public override int GetUniqueHashCode()
        {
            return m_PointData.Position.GetHashCode();
        }

        public override bool UniqueEquals(PointPositionUniquenessValidator validator)
        {
            return m_PointData.Position == validator.m_PointData.Position;
        }
    }
}