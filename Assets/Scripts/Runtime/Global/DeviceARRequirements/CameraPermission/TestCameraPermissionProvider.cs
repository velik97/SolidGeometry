using System;
using UnityEngine;
using Util;

namespace Runtime.Global.DeviceARRequirements.CameraPermission
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