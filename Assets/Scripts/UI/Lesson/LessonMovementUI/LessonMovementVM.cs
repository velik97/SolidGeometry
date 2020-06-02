using Runtime.Lesson;
using UI.MVVM;
using UnityEngine;
using Util.EventBusSystem;

namespace UI.Lesson.LessonMovementUI
{
    public class LessonMovementVM : ViewModel
    {
        public void RotateAroundXY(Vector3 deltaRotation)
        {
            EventBus.RaiseEvent<ILessonMovementHandler>(h => h.HandleRotateAroundXY(deltaRotation));
        }

        public void RotateAroundZ(float deltaRotation)
        {
            EventBus.RaiseEvent<ILessonMovementHandler>(h => h.HandleRotateAroundZ(deltaRotation));
        }

        public void Shift(Vector3 deltaDirection)
        {
            EventBus.RaiseEvent<ILessonMovementHandler>(h => h.HandleShift(deltaDirection));
        }

        public void Scale(float deltaScale)
        {
            EventBus.RaiseEvent<ILessonMovementHandler>(h => h.HandleScale(deltaScale));
        }

        public void Reset()
        {
            EventBus.RaiseEvent<ILessonMovementHandler>(h => h.HandleReset());
        }
    }
}