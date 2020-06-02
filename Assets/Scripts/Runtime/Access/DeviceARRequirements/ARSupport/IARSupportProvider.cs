using System;
using UniRx;
using UnityEngine.XR.ARFoundation;

namespace Runtime.Access.DeviceARRequirements.ARSupport
{
    public interface IARSupportProvider
    {
        IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported{ get; }

        IReadOnlyReactiveProperty<bool> ARIsReady{ get; }

        IReadOnlyReactiveProperty<bool> NeedInstall{ get; }

        IReadOnlyReactiveProperty<bool> IsInstalling{ get; }
        
        void InstallARSoftware();
    }
}