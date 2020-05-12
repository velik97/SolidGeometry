using UniRx;

namespace Runtime.Global.DeviceARRequirements.ARSupport
{
    public class CrossPlatformARSupportProvider : IARSupportProvider
    {
        private IARSupportProvider m_InnerARSupportProvider;

        public CrossPlatformARSupportProvider()
        {
#if UNITY_EDITOR
            m_InnerARSupportProvider = TestARSupportProvider.Instance;
#elif UNITY_IOS || UNITY_ANDROID
            m_InnerARSupportProvider = new MobileARSupportProvider();
#else
            m_InnerARSupportProvider = new UnsupportedARSupportProvider();
#endif
        }

        public IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported => m_InnerARSupportProvider.ARIsCheckedAndSupported;

        public IReadOnlyReactiveProperty<bool> ARIsReady => m_InnerARSupportProvider.ARIsReady;

        public IReadOnlyReactiveProperty<bool> NeedARInstall => m_InnerARSupportProvider.NeedARInstall;

        public IReadOnlyReactiveProperty<bool> IsInstalling => m_InnerARSupportProvider.IsInstalling;
        
        public void InstallARSoftware()
        {
            m_InnerARSupportProvider.InstallARSoftware();
        }
    }
}