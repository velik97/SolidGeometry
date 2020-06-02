using System;
using System.Collections.Generic;
using Runtime.Access.DeviceARRequirements;
using UI.MVVM;
using UniRx;

namespace UI.Lesson.ARRequirementsManual
{
    public class ManualVM : ViewModel
    {
        private readonly ReactiveProperty<int> m_CurrentPageNumber = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> CurrentPageNumber => m_CurrentPageNumber;
        
        private readonly Action m_SucceededAction;

        private List<ManualPageVM> m_ManualPageVMs;
        public IList<ManualPageVM> ManualPageVMs => m_ManualPageVMs;

        public ManualVM(Action succeededAction, Action closeAction, DeviceARRequirementsAccess deviceARRequirementsAccess)
        {
            m_SucceededAction = succeededAction;

            bool needCameraAccess = !deviceARRequirementsAccess.CameraPermissionProvider.HaveCameraPermission();
            bool needInstall = deviceARRequirementsAccess.ARSupportProvider.NeedInstall.Value;

            int count = (needCameraAccess ? 1 : 0) + (needInstall ? 1 : 0);
            m_ManualPageVMs = new List<ManualPageVM>(count);

            int num = 0;
            if (needCameraAccess)
            {
                ManualPageVM pageVM = new CameraPermissionManualPageVM(num, count,
                    closeAction, GoToNextPage, deviceARRequirementsAccess.CameraPermissionProvider);
                AddDisposable(pageVM);
                m_ManualPageVMs.Add(pageVM);
                num++;
            }

            if (needInstall)
            {
                ManualPageVM pageVM = new InstallARSoftwareManualPageVM(num, count,
                    closeAction, GoToNextPage, deviceARRequirementsAccess.ARSupportProvider);
                AddDisposable(pageVM);
                m_ManualPageVMs.Add(pageVM);
            }
        }

        private void GoToNextPage()
        {
            if (m_CurrentPageNumber.Value == m_ManualPageVMs.Count - 1)
            {
                m_SucceededAction?.Invoke();
                return;
            }

            m_CurrentPageNumber.Value++;
        }
    }
}