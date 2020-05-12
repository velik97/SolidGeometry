using System;
using Runtime.Global.DeviceARRequirements;
using Runtime.Global.DeviceARRequirements.ARSupport;
using UniRx;

namespace UI.Session.ARRequirementsManual
{
    public class InstallARSoftwareManualPageVM : ManualPageVM
    {
        private readonly IARSupportProvider m_ARSupportProvider;
        
        public override IReadOnlyReactiveProperty<bool> ManualIsCompleted => m_ARSupportProvider.ARIsReady;
        public IReadOnlyReactiveProperty<bool> NeedInstall => m_ARSupportProvider.NeedARInstall;
        public IReadOnlyReactiveProperty<bool> IsInstalling => m_ARSupportProvider.IsInstalling;

        public InstallARSoftwareManualPageVM(int pageNumber, int pagesCount, Action closeAction, Action goFurtherAction,
            IARSupportProvider arSupportProvider)
            : base(pageNumber, pagesCount, closeAction, goFurtherAction)
        {
            m_ARSupportProvider = arSupportProvider;
        }

        public void Install()
        {
            m_ARSupportProvider.InstallARSoftware();
        }
    }
}