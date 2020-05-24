using System;

namespace Runtime.Global.DeviceARRequirements.CameraPermission
{
    public class UnsupportedCameraPermissionProvider : ICameraPermissionProvider
    {
        public bool HaveCameraPermission()
        {
            return false;
        }

        public void RequestCameraPermission(Action callback)
        {
            callback?.Invoke();
        }

        public void GoToPermissionSettings() { }
    }
}