using Runtime.Global.DeviceARRequirements.ARSupport;
using Runtime.Global.DeviceARRequirements.CameraPermission;
using UniRx;

namespace Runtime.Global.DeviceARRequirements
{
    public class DeviceARRequirementsAccess : CompositeDisposable
    {
        public static DeviceARRequirementsAccess Instance => GlobalAccess.Instance.DeviceARRequirementsAccess; 
        
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