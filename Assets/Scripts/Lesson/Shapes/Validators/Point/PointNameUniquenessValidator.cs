using Lesson.Shapes.Validators.Uniqueness;
using Shapes.Data;

namespace Lesson.Shapes.Validators.Point
{
    public class PointNameUniquenessValidator : UniquenessValidator<PointNameUniquenessValidator>
    {
        private readonly PointData m_PointData;
        
        public PointNameUniquenessValidator(PointData pointData)
        {
            m_PointData = pointData;
            m_PointData.NameUpdated += OnUniqueDeterminingPropertyUpdated;
        }

        public override string GetNotValidMessage()
        {
            return $"Name '{m_PointData.PointName}' is already taken";
        }

        public override int GetUniqueHashCode()
        {
            return m_PointData.PointName.GetHashCode();
        }

        public override bool UniqueEquals(PointNameUniquenessValidator validator)
        {
            return m_PointData.PointName == validator.m_PointData.PointName;
        }
    }
}