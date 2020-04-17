using UnityEngine;
using Util.EventBusSystem;

namespace Runtime.Session
{
    public interface ILessonMovementHandler : IGlobalSubscriber
    {
        void HandleRotateAroundXY(Vector3 deltaRotation);

        void HandleRotateAroundZ(float deltaRotation);

        void HandleShift(Vector3 deltaDirection);

        void HandleScale(float deltaScale);

        void HandleReset();
    }
}