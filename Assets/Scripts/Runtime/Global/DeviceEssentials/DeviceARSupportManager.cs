using UniRx;
using UnityEngine.XR.ARFoundation;
using Util;

namespace Runtime.Global.DeviceEssentials
{
    public class DeviceARSupportManager : CompositeDisposable
    {
        public readonly ReadOnlyReactiveProperty<ARSessionState> CurrentARSessionState;

        public readonly ReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported;

        public readonly ReadOnlyReactiveProperty<bool> ARIsReady;

        public readonly ReadOnlyReactiveProperty<bool> NeedARInstall;

        public readonly ReadOnlyReactiveProperty<bool> IsInstalling;
        
        public DeviceARSupportManager()
        {
            CurrentARSessionState = Observable
                .FromEvent<ARSessionStateChangedEventArgs>(
                    h => ARSession.stateChanged += h,
                    h => ARSession.stateChanged -= h)
                .Select(args => args.state)
                .ToReadOnlyReactiveProperty(ARSession.state);

            ARIsCheckedAndSupported = CurrentARSessionState
                .Select(state =>
                    !(state == ARSessionState.None || state == ARSessionState.CheckingAvailability || state == ARSessionState.Unsupported))
                .ToReadOnlyReactiveProperty();

            ARIsReady = CurrentARSessionState
                .Select(state =>
                    state == ARSessionState.Ready || state == ARSessionState.SessionInitializing || state == ARSessionState.SessionTracking)
                .ToReadOnlyReactiveProperty();

            NeedARInstall = CurrentARSessionState
                .Select(state => state == ARSessionState.NeedsInstall)
                .ToReadOnlyReactiveProperty();
            
            IsInstalling = CurrentARSessionState
                .Select(state => state == ARSessionState.Installing)
                .ToReadOnlyReactiveProperty();
            
            Add(ARIsCheckedAndSupported);
            Add(ARIsReady);
            Add(NeedARInstall);
            Add(IsInstalling);
            Add(CurrentARSessionState);

            CoroutineRunner.Run(ARSession.CheckAvailability());
        }

        public void InstallARSoftware()
        {
            CoroutineRunner.Run(ARSession.Install());
        }
    }
}