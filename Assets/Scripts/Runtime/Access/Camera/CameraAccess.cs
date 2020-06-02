using UniRx;
using UnityEngine;
using Util.UniRxExtensions;

namespace Runtime.Access.Camera
{
    public class CameraAccess : MultipleDisposable
    {
        public static CameraAccess Instance => RootAccess.Instance.CameraAccess;

        private UnityEngine.Camera m_Camera;
        private Transform m_CameraTransform;
        private bool m_HasCamera = false;

        public bool HasCamera => m_HasCamera;

        public Transform CameraTransform => m_CameraTransform;
        public UnityEngine.Camera Camera => m_Camera;

        public void SetCamera(UnityEngine.Camera cameraToSet)
        {
            if (cameraToSet == null)
            {
                return;
            }

            m_Camera = cameraToSet;
            m_CameraTransform = cameraToSet.transform;
            m_HasCamera = true;
        }

        public void DeleteCamera(UnityEngine.Camera cameraToDelete)
        {
            if (cameraToDelete != m_Camera)
            {
                return;
            }

            m_Camera = null;
            m_CameraTransform = null;
            m_HasCamera = false;
        }
    }
}