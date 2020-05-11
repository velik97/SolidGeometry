using System;
using Runtime.Global.DeviceEssentials;
using UniRx;

namespace UI.Session.ARRequirementsManual
{
    public class InstallARSoftwareManualPageVM : ManualPageVM
    {
        private readonly DeviceARSupportManager m_DeviceARSupportManager;
        
        public override IReadOnlyReactiveProperty<bool> ManualIsCompleted => m_DeviceARSupportManager.ARIsReady;
        public IReadOnlyReactiveProperty<bool> NeedInstall => m_DeviceARSupportManager.NeedARInstall;
        public IReadOnlyReactiveProperty<bool> IsInstalling => m_DeviceARSupportManager.IsInstalling;

        public InstallARSoftwareManualPageVM(int pageNumber, int pagesCount, Action closeAction, Action goFurtherAction,
            DeviceARSupportManager deviceARSupportManager)
            : base(pageNumber, pagesCount, closeAction, goFurtherAction)
        {
            m_DeviceARSupportManager = deviceARSupportManager;
        }

        public void Install()
        {
            m_DeviceARSupportManager.InstallARSoftware();
        }
    }
}