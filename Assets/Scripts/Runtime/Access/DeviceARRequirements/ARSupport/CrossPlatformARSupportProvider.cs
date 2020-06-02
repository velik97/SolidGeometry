using UniRx;
using Util.UniRxExtensions;

namespace Runtime.Access.DeviceARRequirements.ARSupport
{
    public class CrossPlatformARSupportProvider : MultipleDisposable, IARSupportProvider
    {
        private readonly IARSupportProvider m_InnerARSupportProvider;

        public CrossPlatformARSupportProvider()
        {
#if UNITY_EDITOR
            TestARSupportProvider provider = new TestARSupportProvider();
            AddDisposable(provider);
            m_InnerARSupportProvider = provider;
#elif PLATFORM_IOS || UNITY_ANDROID
            m_InnerARSupportProvider = new MobileARSupportProvider();
#else
            m_InnerARSupportProvider = new UnsupportedARSupportProvider();
#endif
        }

        public IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported => m_InnerARSupportProvider.ARIsCheckedAndSupported;

        public IReadOnlyReactiveProperty<bool> ARIsReady => m_InnerARSupportProvider.ARIsReady;

        public IReadOnlyReactiveProperty<bool> NeedInstall => m_InnerARSupportProvider.NeedInstall;

        public IReadOnlyReactiveProperty<bool> IsInstalling => m_InnerARSupportProvider.IsInstalling;
        
        public void InstallARSoftware()
        {
            m_InnerARSupportProvider.InstallARSoftware();
        }
    }
}