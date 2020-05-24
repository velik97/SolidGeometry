using System;

namespace Runtime.Global.DeviceARRequirements.CameraPermission
{
    public class MobileCameraPermissionProvider : ICameraPermissionProvider
    {
        public bool HaveCameraPermission()
        {
            return NativeCamera.CheckPermission() == NativeCamera.Permission.Granted;
        }

        public void RequestCameraPermission(Action callback)
        {
            if (HaveCameraPermission())
            {
                callback?.Invoke();
                return;
            }
            NativeCamera.RequestPermission();
            callback?.Invoke();
        }

        public void GoToPermissionSettings()
        {
            if (NativeCamera.CanOpenSettings())
            {
                NativeCamera.OpenSettings();
            }
        }
    }
}