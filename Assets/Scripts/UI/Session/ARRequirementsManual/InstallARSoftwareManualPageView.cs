using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Session.ARRequirementsManual
{
    public class InstallARSoftwareManualPageView : ManualPageView<InstallARSoftwareManualPageVM>
    {
        [SerializeField]
        private GameObject m_NeedInstallGameObject;
        [SerializeField]
        private GameObject m_InstallingGameObject;
        [SerializeField]
        private GameObject m_InstallCompletedGameObject;

        [SerializeField]
        private Button m_InstallButton;

        public override void Bind(InstallARSoftwareManualPageVM viewModel)
        {
            base.Bind(viewModel);
            
            Add(ViewModel.NeedInstall.Subscribe(m_NeedInstallGameObject.SetActive));
            Add(ViewModel.IsInstalling.Subscribe(m_InstallingGameObject.SetActive));
            Add(ViewModel.ManualIsCompleted.Subscribe(m_InstallCompletedGameObject.SetActive));
            
            Add(m_InstallButton.OnClickAsObservable().Subscribe(_ => ViewModel.Install()));
        }

        protected override string GetNotCompletedString()
        {
            return "Без приложения никак!";
        }
    }
}