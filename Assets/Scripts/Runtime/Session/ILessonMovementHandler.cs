using UnityEngine;
using Util.EventBusSystem;

namespace Runtime.Session
{
    public interface ILessonMovementHandler :
        ILessonRotateAroundXYHandler,
        ILessonRotateAroundZHandler,
        ILessonShiftHandler,
        ILessonScaleHandler,
        ILessonResetHandler
    { }

    public interface ILessonRotateAroundXYHandler : IGlobalSubscriber
    {
        void HandleRotateAroundXY(Vector3 deltaRotation);
    }
    
    public interface ILessonRotateAroundZHandler : IGlobalSubscriber
    {
        void HandleRotateAroundZ(float deltaRotation);
    }
    
    public interface ILessonShiftHandler : IGlobalSubscriber
    {
        void HandleShift(Vector3 deltaDirection);
    }
    
    public interface ILessonScaleHandler : IGlobalSubscriber
    {
        void HandleScale(float deltaScale);
    }
    
    public interface ILessonResetHandler : IGlobalSubscriber
    {
        void HandleReset();
    }
}