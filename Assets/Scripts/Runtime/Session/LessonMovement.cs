using DG.Tweening;
using Runtime.CameraManagement;
using UniRx;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Runtime.Session
{
    public class LessonMovement : CompositeDisposable, ILessonMovementHandler
    {
        private const float ROTATE_XY_COEFFICIENT = 2.4f;
        private const float SHIFT_COEFFICIENT = 0.0008f;
        private const float MIN_SCALE = 0.5f;
        private const float MAX_SCALE = 10.0f;
        private const float MAX_SHIFT = 2.5f;

        private Transform m_ShapesAnchor;

        public LessonMovement(Transform shapesAnchor)
        {
            Add(EventBus.Subscribe(this));
            Add(Disposable.Create(ResetTransform));
            
            m_ShapesAnchor = shapesAnchor;
        }
 
        public void HandleRotateAroundXY(Vector3 deltaRotation)
        {
            if (Application.isEditor && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                HandleShift(deltaRotation);
                return;
            }
            
            if (!CameraOwner.Instance.HasCamera)
            {
                return;
            }

            float distanceToCamera = (CameraOwner.Instance.CameraTransform.position - m_ShapesAnchor.transform.position).magnitude;
            distanceToCamera = Mathf.Max(distanceToCamera, 0.1f);

            deltaRotation /= distanceToCamera;
            deltaRotation *= ROTATE_XY_COEFFICIENT;
            
            m_ShapesAnchor.Rotate(CameraOwner.Instance.CameraTransform.up, -deltaRotation.x, Space.World);
            m_ShapesAnchor.Rotate(CameraOwner.Instance.CameraTransform.right, deltaRotation.y, Space.World);
        }

        public void HandleRotateAroundZ(float deltaRotation)
        {
            if (!CameraOwner.Instance.HasCamera)
            {
                return;
            }
            
            m_ShapesAnchor.Rotate(CameraOwner.Instance.CameraTransform.forward, deltaRotation, Space.World);
        }

        public void HandleShift(Vector3 deltaDirection)
        {
            if (!CameraOwner.Instance.HasCamera)
            {
                return;
            }
            
            float distanceToCamera = (CameraOwner.Instance.CameraTransform.position - m_ShapesAnchor.transform.position).magnitude;
            distanceToCamera = Mathf.Max(distanceToCamera, 0.01f);

            deltaDirection *= distanceToCamera;
            deltaDirection *= SHIFT_COEFFICIENT;
            
            Vector3 globalDirection =
                CameraOwner.Instance.CameraTransform.right * deltaDirection.x +
                CameraOwner.Instance.CameraTransform.up * deltaDirection.y;

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

        private void ResetTransform()
        {
            m_ShapesAnchor.Reset();
        }
    }
}