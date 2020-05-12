#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections;
using Plugins.IOSNativePlugins;
using UnityEngine;
using Util;

namespace Runtime.Global.DeviceARRequirements.CameraPermission
{
    public class IOSCameraPermissionProvider : ICameraPermissionProvider
    {
        public bool HaveCameraPermission()
        {
            return Application.HasUserAuthorization(UserAuthorization.WebCam);
        }

        public void RequestCameraPermission(Action callback)
        {
            CoroutineRunner.Run(RequestCameraPermissionCoroutine(callback));
        }

        private IEnumerator RequestCameraPermissionCoroutine(Action callback)
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            callback?.Invoke();
        }

        public void GoToPermissionSettings()
        {
            IOSGoToPermissionSettingsNativeWrapper.OpenSettings();
        }
    }
}
#endif