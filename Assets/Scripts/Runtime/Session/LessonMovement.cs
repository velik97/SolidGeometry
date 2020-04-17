using DG.Tweening;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;

namespace Runtime.Session
{
    public class LessonMovement : CompositeDisposable, ILessonMovementHandler
    {
        private Transform m_ShapesAnchor;
        private Transform m_MainCameraTransform;

        public LessonMovement(Transform shapesAnchor)
        {
            Add(EventBus.Subscribe(this));
            
            m_ShapesAnchor = shapesAnchor;
            m_MainCameraTransform = Camera.main.transform;
        }

        public void HandleRotateAroundXY(Vector3 deltaRotation)
        {
            m_ShapesAnchor.Rotate(m_MainCameraTransform.up, -deltaRotation.x, Space.World);
            m_ShapesAnchor.Rotate(m_MainCameraTransform.right, deltaRotation.y, Space.World);
        }

        public void HandleRotateAroundZ(float deltaRotation)
        {
            m_ShapesAnchor.Rotate(m_MainCameraTransform.forward, deltaRotation, Space.World);
        }

        public void HandleShift(Vector3 deltaDirection)
        {
            Vector3 globalDirection =
                m_MainCameraTransform.right * deltaDirection.x +
                m_MainCameraTransform.up * deltaDirection.y;

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
    }
}