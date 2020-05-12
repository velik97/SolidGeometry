using UniRx;

namespace Runtime.Global.DeviceARRequirements.ARSupport
{
    public interface IARSupportProvider
    {
        IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported{ get; }

        IReadOnlyReactiveProperty<bool> ARIsReady{ get; }

        IReadOnlyReactiveProperty<bool> NeedARInstall{ get; }

        IReadOnlyReactiveProperty<bool> IsInstalling{ get; }

        void InstallARSoftware();
    }
}