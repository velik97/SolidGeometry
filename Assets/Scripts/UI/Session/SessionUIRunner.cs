using System;
using Lesson;
using Runtime;
using Runtime.Core;
using Runtime.Global;
using Runtime.Global.ApplicationModeManagement;
using Runtime.Global.DeviceARRequirements;
using Runtime.Session;
using UI.Session.ARRequirementsManual;
using UI.Session.CloseLesson;
using UI.Session.LessonBrowserUI;
using UI.Session.LessonMovementUI;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace UI.Session
{
    public class SessionUIRunner : MonoBehaviourCompositeDisposable, ISceneRunner
    {
        [SerializeField]
        private LessonBrowserView m_BrowserView;
        [SerializeField]
        private LessonMovementView m_LessonMovementView;

        [SerializeField]
        private TopPanelView m_TopPanelView;

        [SerializeField]
        private ManualView m_ManualView;
        private ManualVM m_ManualVM;
        
        
        public void Initialize()
        {
            InitializeLessonBrowser();
            InitializeLessonMovement();
            InitializeTopPanel();
            InitializeManual();
        }

        private void InitializeLessonBrowser()
        {
            LessonBrowserVM browserVM = new LessonBrowserVM();
            Add(browserVM);
            m_BrowserView.Bind(browserVM);
        }

        private void InitializeLessonMovement()
        {
            LessonMovementVM movementVM = new LessonMovementVM();
            Add(movementVM);
            m_LessonMovementView.Bind(movementVM);
        }

        private void InitializeTopPanel()
        {
            IReadOnlyReactiveProperty<bool> canChangeMode =
                DeviceARRequirementsAccess.Instance.ARSupportProvider.ARIsCheckedAndSupported
                .Or(ApplicationModeAccess.Instance.CurrentApplicationModeProperty.Select(mode => mode == ApplicationMode.SessionAR))
                .ToReadOnlyReactiveProperty();
            TopPanelVM topPanelVM = new TopPanelVM(GoBack, ChangeMode, canChangeMode);
            Add(topPanelVM);
            m_TopPanelView.Bind(topPanelVM);
        }

        private void InitializeManual()
        {
            m_ManualView.Initialize();
        }
        
        private void GoBack()
        {
            ApplicationModeAccess.Instance.RequestChangeApplicationMode(ApplicationMode.MainMenu);
        }

        private void ChangeMode()
        {
            switch (ApplicationModeAccess.Instance.CurrentApplicationMode)
            {
                case ApplicationMode.Session3D:
                    RequestGoToAR();
                    break;
                case ApplicationMode.SessionAR:
                    ApplicationModeAccess.Instance.RequestChangeApplicationMode(ApplicationMode.Session3D);
                    break;
            }
        }

        private void RequestGoToAR()
        {
            bool haveCameraAccess = DeviceARRequirementsAccess.Instance.CameraPermissionProvider.HaveCameraPermission();
            if (!haveCameraAccess)
            {
                DeviceARRequirementsAccess.Instance.CameraPermissionProvider.RequestCameraPermission(CheckRequirementsAndTryGoToAR);
                return;
            }
            CheckRequirementsAndTryGoToAR();
        }

        private void CheckRequirementsAndTryGoToAR()
        {
            bool haveCameraAccess = DeviceARRequirementsAccess.Instance.CameraPermissionProvider.HaveCameraPermission();
            bool arIsReady = DeviceARRequirementsAccess.Instance.ARSupportProvider.ARIsReady.Value;

            if (haveCameraAccess && arIsReady)
            {
                ApplicationModeAccess.Instance.RequestChangeApplicationMode(ApplicationMode.SessionAR);
                return;
            }

            StartARRequirementsManual();
        }

        private void StartARRequirementsManual()
        {
            m_ManualVM = new ManualVM(StartARAfterManual, CloseARManual, DeviceARRequirementsAccess.Instance);
            m_ManualView.Bind(m_ManualVM);
        }

        private void CloseARManual()
        {
            m_ManualVM?.Dispose();
        }

        private void StartARAfterManual()
        {
            m_ManualVM?.Dispose();
            ApplicationModeAccess.Instance.RequestChangeApplicationMode(ApplicationMode.SessionAR);
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}