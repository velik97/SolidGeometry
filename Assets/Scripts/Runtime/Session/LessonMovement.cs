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
        private Transform m_ShapesAnchor;

        public LessonMovement(Transform shapesAnchor)
        {
            Add(EventBus.Subscribe(this));
            Add(Disposable.Create(ResetTransform));
            
            m_ShapesAnchor = shapesAnchor;
        }

        public void HandleRotateAroundXY(Vector3 deltaRotation)
        {
            if (!CameraOwner.Instance.HasCamera)
            {
                return;
            }
            
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
            
            Vector3 globalDirection =
                CameraOwner.Instance.CameraTransform.right * deltaDirection.x +
                CameraOwner.Instance.CameraTransform.up * deltaDirection.y;

            m_ShapesAnchor.Translate(globalDirection);
        }

        public void HandleScale(float deltaScale)
        {
            m_ShapesAnchor.localScale *= deltaScale;
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