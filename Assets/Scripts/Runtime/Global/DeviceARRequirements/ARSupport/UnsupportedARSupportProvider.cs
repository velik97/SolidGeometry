using UniRx;

namespace Runtime.Global.DeviceARRequirements.ARSupport
{
    public class UnsupportedARSupportProvider : IARSupportProvider
    {
        public IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported { get; } = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> ARIsReady { get; } = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> NeedARInstall { get; } = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsInstalling { get; } = new BoolReactiveProperty(false);

        public void InstallARSoftware() { }
    }
}