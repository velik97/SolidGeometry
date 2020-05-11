using System;

namespace Runtime.Global.DeviceEssentials
{
    public class CrossPlatformCameraPermissionProvider : ICameraPermissionProvider
    {
        private ICameraPermissionProvider m_InnerCameraPermissionProvider;

        public CrossPlatformCameraPermissionProvider()
        {
#if UNITY_EDITOR
            m_InnerCameraPermissionProvider = new EditorCameraPermissionProvider();
#elif UNITY_IOS && !UNITY_EDITOR
            m_InnerCameraPermissionProvider = new IOSCameraPermissionProvider();
#elif UNITY_ANDROID && !UNITY_EDITOR
            m_InnerCameraPermissionProvider = new AndroidCameraPermissionProvider();
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