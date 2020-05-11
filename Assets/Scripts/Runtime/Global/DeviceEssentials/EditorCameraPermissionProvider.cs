using System;

namespace Runtime.Global.DeviceEssentials
{
    public class EditorCameraPermissionProvider : ICameraPermissionProvider
    {
        public bool HaveCameraPermission()
        {
            return true;
        }

        public void RequestCameraPermission(Action callback)
        {
            callback?.Invoke();
        }

        public void GoToPermissionSettings()
        {
            return;
        }
    }
}