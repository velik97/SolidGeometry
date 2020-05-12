using UniRx;
using UnityEngine.XR.ARFoundation;
using Util;

namespace Runtime.Global.DeviceARRequirements.ARSupport
{
    public class MobileARSupportProvider : CompositeDisposable, IARSupportProvider
    {
        public IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported { get; }
        public IReadOnlyReactiveProperty<bool> ARIsReady { get; }
        public IReadOnlyReactiveProperty<bool> NeedARInstall { get; }
        public IReadOnlyReactiveProperty<bool> IsInstalling { get; }
        
        public MobileARSupportProvider()
        {
            var currentARSessionState = Observable
                .FromEvent<ARSessionStateChangedEventArgs>(
                    h => ARSession.stateChanged += h,
                    h => ARSession.stateChanged -= h)
                .Select(args => args.state)
                .ToReadOnlyReactiveProperty(ARSession.state);

            ARIsCheckedAndSupported = currentARSessionState
                .Select(state =>
                    !(state == ARSessionState.None || state == ARSessionState.CheckingAvailability || state == ARSessionState.Unsupported))
                .ToReadOnlyReactiveProperty();

            ARIsReady = currentARSessionState
                .Select(state =>
                    state == ARSessionState.Ready || state == ARSessionState.SessionInitializing || state == ARSessionState.SessionTracking)
                .ToReadOnlyReactiveProperty();

            NeedARInstall = currentARSessionState
                .Select(state => state == ARSessionState.NeedsInstall)
                .ToReadOnlyReactiveProperty();
            
            IsInstalling = currentARSessionState
                .Select(state => state == ARSessionState.Installing)
                .ToReadOnlyReactiveProperty();
            
            Add(currentARSessionState);

            CoroutineRunner.Run(ARSession.CheckAvailability());
        }

        public void InstallARSoftware()
        {
            CoroutineRunner.Run(ARSession.Install());
        }
    }
}