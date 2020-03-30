using Session;
using UI.MVVM;
using UnityEngine;

namespace UI.Session.LessonMovementUI
{
    public class LessonMovementVM : ViewModel
    {
        private LessonMovement m_LessonMovement;

        public LessonMovementVM(LessonMovement lessonMovement)
        {
            m_LessonMovement = lessonMovement;
        }
        
        public void RotateAroundXY(Vector3 deltaRotation)
        {
            m_LessonMovement.RotateAroundXY(deltaRotation);
        }

        public void RotateAroundZ(float deltaRotation)
        {
            m_LessonMovement.RotateAroundZ(deltaRotation);
        }

        public void Shift(Vector3 deltaDirection)
        {
            m_LessonMovement.Shift(deltaDirection);
        }

        public void Scale(float deltaScale)
        {
            m_LessonMovement.Scale(deltaScale);
        }

        public void Reset()
        {
            m_LessonMovement.Reset();
        }
    }
}