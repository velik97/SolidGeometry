using Runtime.Access.DeviceARRequirements.ARSupport;
using Runtime.Access.DeviceARRequirements.CameraPermission;
using UniRx;
using Util.UniRxExtensions;

namespace Runtime.Access.DeviceARRequirements
{
    public class DeviceARRequirementsAccess : MultipleDisposable
    {
        public static DeviceARRequirementsAccess Instance => RootAccess.Instance.DeviceARRequirementsAccess; 
        
        public readonly IARSupportProvider ARSupportProvider;
        public readonly ICameraPermissionProvider CameraPermissionProvider;

        public DeviceARRequirementsAccess()
        {
            ARSupportProvider = new CrossPlatformARSupportProvider();
            CameraPermissionProvider = new CrossPlatformCameraPermissionProvider();
        }

        public bool ReadyForAR()
        {
            return ARSupportProvider.ARIsReady.Value && CameraPermissionProvider.HaveCameraPermission();
        }
    }
}