using Lesson.Shapes.Blueprints.DependentShapes;
using UnityEngine;
using Util;

namespace Lesson.Validators.Projections
{
    public class ProjectionAlongLineValidator : Validator
    {
        private PointProjectionAlongLineBlueprint m_Blueprint;

        public ProjectionAlongLineValidator(PointProjectionAlongLineBlueprint blueprint)
        {
            m_Blueprint = blueprint;
        }

        protected override bool CheckIsValid()
        {
            if (m_Blueprint.FirstPointOnLine == null 
                || m_Blueprint.ProjectedPoint == null 
                || m_Blueprint.SecondPointOnLine == null 
                || m_Blueprint.FirstPointAlong == null 
                || m_Blueprint.SecondPointAlong == null)
            {
                return true;
            }

            Vector3 targetLineVector = m_Blueprint.SecondPointOnLine.Position -
                                       m_Blueprint.FirstPointOnLine.Position;
            Vector3 parallelLineVector = m_Blueprint.SecondPointAlong.Position -
                                         m_Blueprint.FirstPointAlong.Position;
            Vector3 sourceToTargetVector = m_Blueprint.ProjectedPoint.Position -
                                           m_Blueprint.FirstPointOnLine.Position;

            return targetLineVector.CollinearWith(parallelLineVector, sourceToTargetVector);
        }

        public override string GetNotValidMessage()
        {
            return $"Line {m_Blueprint.FirstPointAlong.PointName}{m_Blueprint.SecondPointAlong.PointName} has to be parallel to the plane " +
                   $"{m_Blueprint.ProjectedPoint.PointName}{m_Blueprint.FirstPointOnLine.PointName}{m_Blueprint.SecondPointOnLine.PointName}";
        }
    }
}