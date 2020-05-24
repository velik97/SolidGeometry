using System;

namespace Runtime.Global.DeviceARRequirements.CameraPermission
{
    public class CrossPlatformCameraPermissionProvider : ICameraPermissionProvider
    {
        private readonly ICameraPermissionProvider m_InnerCameraPermissionProvider;

        public CrossPlatformCameraPermissionProvider()
        {
#if UNITY_EDITOR
            m_InnerCameraPermissionProvider = new TestCameraPermissionProvider();
#elif UNITY_IOS || UNITY_ANDROID
            m_InnerCameraPermissionProvider = new MobileCameraPermissionProvider();
#else
            m_InnerCameraPermissionProvider = new UnsupportedCameraPermissionProvider();
#endif
        }

        public bool HaveCameraPermission()
        {
            return m_InnerCameraPermissionProvider.HaveCameraPermission();
        }

        public void RequestCameraPermission(Action callback)
        {
            m_InnerCameraPermissionProvider.RequestCameraPermission(callback);
        }

        public void GoToPermissionSettings()
        {
            m_InnerCameraPermissionProvider.GoToPermissionSettings();
        }
    }
}