using System;
using Runtime.Access.DeviceARRequirements.ARSupport;
using UniRx;

namespace UI.Lesson.ARRequirementsManual
{
    public class InstallARSoftwareManualPageVM : ManualPageVM
    {
        private readonly IARSupportProvider m_ARSupportProvider;
        
        public override IReadOnlyReactiveProperty<bool> ManualIsCompleted => m_ARSupportProvider.ARIsReady;
        public IReadOnlyReactiveProperty<bool> NeedInstall => m_ARSupportProvider.NeedInstall;
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