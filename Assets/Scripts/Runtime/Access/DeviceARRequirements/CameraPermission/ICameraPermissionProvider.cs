using System;

namespace Runtime.Access.DeviceARRequirements.CameraPermission
{
    public interface ICameraPermissionProvider
    {
        bool HaveCameraPermission();

        void RequestCameraPermission(Action callback);

        void GoToPermissionSettings();
    }
}