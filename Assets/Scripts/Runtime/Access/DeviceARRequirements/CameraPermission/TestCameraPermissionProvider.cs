using System;

namespace Runtime.Access.DeviceARRequirements.CameraPermission
{
    public class TestCameraPermissionProvider : ICameraPermissionProvider
    {
        private bool m_HaveCameraPermission = false;

        public bool HaveCameraPermission()
        {
            return m_HaveCameraPermission;
        }

        public void RequestCameraPermission(Action callback)
        {
            callback?.Invoke();
        }

        public void GoToPermissionSettings()
        {
            m_HaveCameraPermission = true;
        }
    }
}