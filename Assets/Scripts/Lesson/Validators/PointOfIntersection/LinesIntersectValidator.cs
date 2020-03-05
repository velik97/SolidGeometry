using Shapes.Blueprint.DependentShapes;
using Util;

namespace Lesson.Validators.PointOfIntersection
{
    public class LinesIntersectValidator : Validator
    {
        private PointOfIntersectionBlueprint m_Blueprint;

        public LinesIntersectValidator(PointOfIntersectionBlueprint blueprint)
        {
            m_Blueprint = blueprint;
        }

        protected override bool CheckIsValid()
        {
            return GeometryUtils.LinesIntersect(
                m_Blueprint.PointsOnLines[0][0].Position,
                m_Blueprint.PointsOnLines[0][1].Position,
                m_Blueprint.PointsOnLines[1][0].Position,
                m_Blueprint.PointsOnLines[1][1].Position);
        }

        public override string GetNotValidMessage()
        {
            return "Lines don't intersect";
        }
    }
}