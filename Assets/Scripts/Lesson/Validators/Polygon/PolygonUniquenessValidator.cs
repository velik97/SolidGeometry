using System.Linq;
using Lesson.Shapes.Datas;
using Lesson.Validators.Uniqueness;
using Util;

namespace Lesson.Validators.Polygon
{
    public class PolygonUniquenessValidator : UniquenessValidator<PolygonUniquenessValidator>
    {
        private readonly PolygonData m_PolygonData;
        
        public PolygonUniquenessValidator(PolygonData polygonData)
        {
            m_PolygonData = polygonData;
            m_PolygonData.NameUpdated.Subscribe(OnUniqueDeterminingPropertyUpdated);
        }

        public override string GetNotValidMessage()
        {
            return $"{m_PolygonData} is not unique";
        }

        public override int GetUniqueHashCode()
        {
            return m_PolygonData.Points.Aggregate(0, (current, pointData) => current ^ (pointData?.GetHashCode() ?? 0));
        }

        public override bool UniqueEquals(PolygonUniquenessValidator validator)
        {
            return m_PolygonData.Points.HasSameItems(validator.m_PolygonData.Points);
        }
    }
}