using System;
using Runtime;
using Runtime.Access.ApplicationModeManagement;
using Runtime.Access.DeviceARRequirements;
using UI.Lesson.ARLessonPlacingUI;
using UI.Lesson.ARRequirementsManual;
using UI.Lesson.LessonBrowserUI;
using UI.Lesson.LessonMovementUI;
using UI.Lesson.TopPanel;
using UniRx;
using UnityEngine;
using Util.EventBusSystem;
using Util.UniRxExtensions;

namespace UI.Lesson
{
    public class SessionUIRunner : MonoBehaviourMultipleDisposable, ISceneRunner, IApplicationModeHandler
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

        [SerializeField]
        private ARLessonPlacingView m_ARLessonPlacingView;
        private ARLessonPlacingVM m_ARLessonPlacingVM;
        
        public void Initialize()
        {
            AddDisposable(EventBus.Subscribe(this));
            
            InitializeLessonBrowser();
            InitializeLessonMovement();
            InitializeTopPanel();
            InitializeManual();
        }

        private void InitializeLessonBrowser()
        {
            LessonBrowserVM browserVM = new LessonBrowserVM();
            AddDisposable(browserVM);
            m_BrowserView.Bind(browserVM);
        }

        private void InitializeLessonMovement()
        {
            LessonMovementVM movementVM = new LessonMovementVM();
            AddDisposable(movementVM);
            m_LessonMovementView.Bind(movementVM);
        }

        private void InitializeTopPanel()
        {
            IReadOnlyReactiveProperty<bool> canChangeMode =
                DeviceARRequirementsAccess.Instance.ARSupportProvider.ARIsCheckedAndSupported
                .Or(ApplicationModeAccess.Instance.CurrentApplicationModeProperty.Select(mode => mode == ApplicationMode.SessionAR))
                .ToReadOnlyReactiveProperty();
            TopPanelVM topPanelVM = new TopPanelVM(GoBack, ChangeMode, canChangeMode);
            AddDisposable(topPanelVM);
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

        public void HandleApplicationModeChanged(ApplicationMode mode)
        {
            switch (mode)
            {
                case ApplicationMode.None:
                case ApplicationMode.MainMenu:
                case ApplicationMode.Session3D:
                    BindARPlacingLesson();
                    break;
                case ApplicationMode.SessionAR:
                    UnbindARPlacingLesson();
                    break;
            }
        }

        private void BindARPlacingLesson()
        {
            AddDisposable(m_ARLessonPlacingVM = new ARLessonPlacingVM());
            m_ARLessonPlacingView.Bind(m_ARLessonPlacingVM);
        }
        
        private void UnbindARPlacingLesson()
        {
            if (m_ARLessonPlacingVM == null)
            {
                return;
            }
            RemoveDisposable(m_ARLessonPlacingVM);
            m_ARLessonPlacingVM.Dispose();
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}