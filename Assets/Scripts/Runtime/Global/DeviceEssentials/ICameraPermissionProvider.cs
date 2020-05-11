using System;

namespace Runtime.Global.DeviceEssentials
{
    public interface ICameraPermissionProvider
    {
        bool HaveCameraPermission();

        void RequestCameraPermission(Action callback);

        void GoToPermissionSettings();
    }
}