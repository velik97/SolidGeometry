using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Util;
using Util.UniRxExtensions;

namespace Runtime.Access.DeviceARRequirements.ARSupport
{
    public class MobileARSupportProvider : MultipleDisposable, IARSupportProvider
    {
        public IReadOnlyReactiveProperty<bool> ARIsCheckedAndSupported { get; }
        public IReadOnlyReactiveProperty<bool> ARIsReady { get; }
        public IReadOnlyReactiveProperty<bool> NeedInstall { get; }
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

            NeedInstall = currentARSessionState
                .Select(state => state == ARSessionState.NeedsInstall)
                .ToReadOnlyReactiveProperty();
            
            IsInstalling = currentARSessionState
                .Select(state => state == ARSessionState.Installing)
                .ToReadOnlyReactiveProperty();
            
            AddDisposable(currentARSessionState);

            CoroutineRunner.Run(CheckAvailability());
        }

        private IEnumerator CheckAvailability()
        {
            ARSession arSession = Object.FindObjectOfType<ARSession>();
            if (arSession == null)
            {
                arSession = new GameObject(typeof(ARSession).ToString()).AddComponent<ARSession>();
            }
            arSession.enabled = false;
            
            yield return new WaitForSeconds(.5f);

            yield return ARSession.CheckAvailability();
        }

        public void InstallARSoftware()
        {
            CoroutineRunner.Run(ARSession.Install());
        }
    }
}