using System;

namespace Runtime.Global.DeviceARRequirements.CameraPermission
{
    public interface ICameraPermissionProvider
    {
        bool HaveCameraPermission();

        void RequestCameraPermission(Action callback);

        void GoToPermissionSettings();
    }
}