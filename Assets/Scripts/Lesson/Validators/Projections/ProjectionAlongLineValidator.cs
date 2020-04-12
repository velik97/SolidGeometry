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
            if (m_Blueprint.FirstPointOnTargetLine == null 
                || m_Blueprint.ProjectedPoint == null 
                || m_Blueprint.SecondPointOnTargetLine == null 
                || m_Blueprint.FirstPointOnParallelLine == null 
                || m_Blueprint.SecondPointOnParallelLine == null)
            {
                return true;
            }

            Vector3 targetLineVector = m_Blueprint.SecondPointOnTargetLine.Position -
                                       m_Blueprint.FirstPointOnTargetLine.Position;
            Vector3 parallelLineVector = m_Blueprint.SecondPointOnParallelLine.Position -
                                         m_Blueprint.FirstPointOnParallelLine.Position;
            Vector3 sourceToTargetVector = m_Blueprint.ProjectedPoint.Position -
                                           m_Blueprint.FirstPointOnTargetLine.Position;

            return targetLineVector.CollinearWith(parallelLineVector, sourceToTargetVector);
        }

        public override string GetNotValidMessage()
        {
            return $"Line {m_Blueprint.FirstPointOnParallelLine.PointName}{m_Blueprint.SecondPointOnParallelLine.PointName} has to be parallel to the plane " +
                   $"{m_Blueprint.ProjectedPoint.PointName}{m_Blueprint.FirstPointOnTargetLine.PointName}{m_Blueprint.SecondPointOnTargetLine.PointName}";
        }
    }
}