using System.Runtime.InteropServices;

namespace Plugins.IOSNativePlugins
{
    public static class IOSGoToPermissionSettingsNativeWrapper
    {
        // Source: https://stackoverflow.com/questions/30010334/open-settings-application-on-unity-ios
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport ("__Internal")]
        public static extern void OpenSettings();
#endif
    }
}