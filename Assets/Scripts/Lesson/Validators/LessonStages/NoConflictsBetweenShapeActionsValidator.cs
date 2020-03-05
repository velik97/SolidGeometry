using Lesson.Stages;

namespace Lesson.Validators.LessonStages
{
    public class NoConflictsBetweenShapeActionsValidator : Validator
    {
        private readonly LessonStage m_LessonStage;

        public NoConflictsBetweenShapeActionsValidator(LessonStage lessonStage)
        {
            m_LessonStage = lessonStage;
            m_LessonStage.ShapeActionsListUpdated += Update;
            Update();
        }

        protected override bool CheckIsValid()
        {
            for (var i = 0; i < m_LessonStage.ShapeActions.Count - 1; i++)
            {
                for (var j = i + 1; j < m_LessonStage.ShapeActions.Count; j++)
                {
                    if (m_LessonStage.ShapeActions[i].HasConflictWith(m_LessonStage.ShapeActions[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override string GetNotValidMessage()
        {
            return "Some actions conflict with each other";
        }
    }
}