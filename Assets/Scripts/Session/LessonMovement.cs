using DG.Tweening;
using UnityEngine;

namespace Session
{
    public class LessonMovement
    {
        private Transform m_ShapesAnchor;
        private Transform m_MainCameraTransform;

        public LessonMovement(Transform shapesAnchor)
        {
            m_ShapesAnchor = shapesAnchor;
            m_MainCameraTransform = Camera.main.transform;
        }

        public void Rotate(Vector2 deltaDirection)
        {
            m_ShapesAnchor.Rotate(m_MainCameraTransform.up, -deltaDirection.x, Space.World);
            m_ShapesAnchor.Rotate(m_MainCameraTransform.right, deltaDirection.y, Space.World);
        }

        public void Shift(Vector2 deltaDirection)
        {
            Vector3 globalDirection =
                m_MainCameraTransform.right * deltaDirection.x +
                m_MainCameraTransform.up * deltaDirection.y;

            m_ShapesAnchor.Translate(globalDirection);
        }

        public void Scale(float deltaScale)
        {
            m_ShapesAnchor.localScale *= deltaScale;
        }

        public void Reset()
        {
            m_ShapesAnchor.DOLocalMove(Vector3.zero, 1f);
            m_ShapesAnchor.DOLocalRotate(Vector3.zero, 1f);
            m_ShapesAnchor.DOScale(Vector3.one, 1f);
        }
    }
}