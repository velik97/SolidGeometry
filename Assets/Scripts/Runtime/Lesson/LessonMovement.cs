using DG.Tweening;
using Runtime.Access.ARLesson;
using Runtime.Access.Camera;
using UniRx;
using UnityEngine;
using Util;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace Runtime.Lesson
{
    public class LessonMovement : MultipleDisposable, ILessonMovementHandler, IARLessonStateHandler
    {
        private const float ROTATE_XY_COEFFICIENT = 2.4f;
        private const float SHIFT_COEFFICIENT = 0.0008f;
        private const float MIN_SCALE = 0.5f;
        private const float MAX_SCALE = 10.0f;
        private const float MAX_SHIFT = 2.5f;

        private Transform m_ShapesAnchor;

        public LessonMovement(Transform shapesAnchor)
        {
            AddDisposable(EventBus.Subscribe(this));
            AddDisposable(Disposable.Create(ResetTransform));
            
            m_ShapesAnchor = shapesAnchor;
        }
 
        public void HandleRotateAroundXY(Vector3 deltaRotation)
        {
            if (!CameraAccess.Instance.HasCamera)
            {
                return;
            }

            float distanceToCamera = (CameraAccess.Instance.CameraTransform.position - m_ShapesAnchor.transform.position).magnitude;
            distanceToCamera = Mathf.Max(distanceToCamera, 0.1f);

            deltaRotation /= distanceToCamera;
            deltaRotation *= ROTATE_XY_COEFFICIENT;
            
            m_ShapesAnchor.Rotate(CameraAccess.Instance.CameraTransform.up, -deltaRotation.x, Space.World);
            m_ShapesAnchor.Rotate(CameraAccess.Instance.CameraTransform.right, deltaRotation.y, Space.World);
        }

        public void HandleRotateAroundZ(float deltaRotation)
        {
            if (!CameraAccess.Instance.HasCamera)
            {
                return;
            }
            
            m_ShapesAnchor.Rotate(CameraAccess.Instance.CameraTransform.forward, deltaRotation, Space.World);
        }

        public void HandleShift(Vector3 deltaDirection)
        {
            if (!CameraAccess.Instance.HasCamera)
            {
                return;
            }
            
            float distanceToCamera = (CameraAccess.Instance.CameraTransform.position - m_ShapesAnchor.transform.position).magnitude;
            distanceToCamera = Mathf.Max(distanceToCamera, 0.01f);

            deltaDirection *= distanceToCamera;
            deltaDirection *= SHIFT_COEFFICIENT;
            
            Vector3 globalDirection =
                CameraAccess.Instance.CameraTransform.right * deltaDirection.x +
                CameraAccess.Instance.CameraTransform.up * deltaDirection.y;

            Vector3 newPosition = m_ShapesAnchor.localPosition + globalDirection;
            newPosition = newPosition.ClampMagnitude(MAX_SHIFT);

            m_ShapesAnchor.position = newPosition;
        }

        public void HandleScale(float deltaScale)
        {
            Vector3 newScale = m_ShapesAnchor.localScale * deltaScale;
            newScale = newScale.ClampComponentWise(MIN_SCALE, MAX_SCALE);
            
            m_ShapesAnchor.localScale = newScale;
        }

        public void HandleReset()
        {
            m_ShapesAnchor.DOLocalMove(Vector3.zero, .5f);
            m_ShapesAnchor.DOLocalRotate(Vector3.zero, .5f);
            m_ShapesAnchor.DOScale(Vector3.one, .5f);
        }

        public void HandleARLessonStateChanged(ARLessonState state)
        {
            HandleReset();
        }

        private void ResetTransform()
        {
            m_ShapesAnchor.Reset();
        }
    }
}