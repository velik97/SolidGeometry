using System;
using Runtime.Access.ApplicationModeManagement;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace Runtime.Access.ARLesson
{
    public class ARLessonAccess : MultipleDisposable
    {
        public static ARLessonAccess Instance => RootAccess.Instance.ARLessonAccess;
        
        private ARLessonState m_ARLessonState = ARLessonState.NotRunning;
        public ARLessonState ARLessonState => m_ARLessonState;

        public ARLessonAccess()
        {
            ReadOnlyReactiveProperty<ARSessionState> currentARSessionState = Observable
                .FromEvent<ARSessionStateChangedEventArgs>(
                    h => ARSession.stateChanged += h,
                    h => ARSession.stateChanged -= h)
                .Select(args => args.state)
                .ToReadOnlyReactiveProperty(ARSession.state);

            AddDisposable(currentARSessionState);
            AddDisposable(currentARSessionState.Subscribe(OnARSessionStateChanged));
        }

        public void RequestPlace()
        {
            if (m_ARLessonState != ARLessonState.Running)
            {
                return;
            }
            if (ARSession.state != ARSessionState.SessionTracking)
            {
                return;
            }
            SetLessonARState(ARLessonState.PlacingLesson);
        }

        public void ConfirmReplace()
        {
            if (m_ARLessonState != ARLessonState.PlacingLesson)
            {
                return;
            }
            SetLessonARState(ARLessonState.Running);
        }

        private void OnARSessionStateChanged(ARSessionState arSessionState)
        {
            switch (arSessionState)
            {
                case ARSessionState.None:
                case ARSessionState.Unsupported:
                case ARSessionState.CheckingAvailability:
                case ARSessionState.NeedsInstall:
                case ARSessionState.Installing:
                case ARSessionState.Ready:
                    SetLessonARState(ARLessonState.NotRunning);
                    break;
                case ARSessionState.SessionInitializing:
                    SetLessonARState(ARLessonState.ExtractingFeaturePoints);
                    break;
                case ARSessionState.SessionTracking:
                    // if we were extracting feature points or were not running, we need to place lesson
                    // if we were placing or running, we should continue do the same
                    if (m_ARLessonState == ARLessonState.ExtractingFeaturePoints || m_ARLessonState == ARLessonState.NotRunning)
                    {
                        SetLessonARState(ARLessonState.PlacingLesson);
                    }
                    break;
            }
        }

        private void SetLessonARState(ARLessonState state)
        {
            if (m_ARLessonState == state)
            {
                return;
            }

            if (state != ARLessonState.NotRunning &&
                ApplicationModeAccess.Instance.CurrentApplicationMode != ApplicationMode.SessionAR)
            {
                Debug.LogError($"Can't change ARLessonState to {state}" +
                               $"while application mode is {ApplicationModeAccess.Instance.CurrentApplicationMode}");
                return;
            }
            m_ARLessonState = state;
            EventBus.RaiseEvent<IARLessonStateHandler>(h => h.HandleARLessonStateChanged(state));
        }
    }

    public enum ARLessonState
    {
        NotRunning,
        ExtractingFeaturePoints,
        PlacingLesson,
        Running
    }
}