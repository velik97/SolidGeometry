using System;
using System.Collections.Generic;
using Runtime.Global.DeviceEssentials;
using UI.MVVM;
using UniRx;

namespace UI.Session.ARRequirementsManual
{
    public class ManualVM : ViewModel
    {
        private readonly ReactiveProperty<int> m_CurrentPageNumber;
        public IReadOnlyReactiveProperty<int> CurrentPageNumber => m_CurrentPageNumber;
        
        private readonly Action m_SucceededAction;

        private List<ManualPageVM> m_ManualPageVMs;
        public IList<ManualPageVM> ManualPageVMs => m_ManualPageVMs;

        public ManualVM(Action succeededAction, Action closeAction, DeviceEssentialsAccess deviceEssentialsAccess)
        {
            m_SucceededAction = succeededAction;

            bool needCameraAccess = !deviceEssentialsAccess.CameraPermissionProvider.HaveCameraPermission();
            bool needInstall = deviceEssentialsAccess.DeviceARSupportManager.NeedARInstall.Value;

            int count = (needCameraAccess ? 1 : 0) + (needInstall ? 1 : 0);
            m_ManualPageVMs = new List<ManualPageVM>(count);

            int num = 0;
            if (needCameraAccess)
            {
                ManualPageVM pageVM = new CameraPermissionManualPageVM(num, count,
                    closeAction, GoToNextPage, deviceEssentialsAccess.CameraPermissionProvider);
                Add(pageVM);
                m_ManualPageVMs.Add(pageVM);
                num++;
            }

            if (needInstall)
            {
                ManualPageVM pageVM = new InstallARSoftwareManualPageVM(num, count,
                    closeAction, GoToNextPage, deviceEssentialsAccess.DeviceARSupportManager);
                Add(pageVM);
                m_ManualPageVMs.Add(pageVM);
            }
        }

        private void GoToNextPage()
        {
            if (m_CurrentPageNumber.Value == m_ManualPageVMs.Count - 1)
            {
                m_SucceededAction?.Invoke();
            }

            m_CurrentPageNumber.Value++;
        }
    }
}