using UniRx;

namespace Runtime.Global.DeviceEssentials
{
    public class DeviceEssentialsAccess : CompositeDisposable
    {
        public static DeviceEssentialsAccess Instance => GlobalAccess.Instance.DeviceEssentialsAccess; 
        
        public readonly DeviceARSupportManager DeviceARSupportManager;
        public readonly ICameraPermissionProvider CameraPermissionProvider;

        public DeviceEssentialsAccess()
        {
            DeviceARSupportManager = new DeviceARSupportManager();
            CameraPermissionProvider = new CrossPlatformCameraPermissionProvider();
            
            Add(DeviceARSupportManager);
        }
    }
}