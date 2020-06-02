using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lesson.ARRequirementsManual
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
            
            AddDisposable(ViewModel.NeedInstall.Subscribe(m_NeedInstallGameObject.SetActive));
            AddDisposable(ViewModel.IsInstalling.Subscribe(m_InstallingGameObject.SetActive));
            AddDisposable(ViewModel.ManualIsCompleted.Subscribe(m_InstallCompletedGameObject.SetActive));
        }

        protected override IEnumerable<IDisposable> GetButtonsBindings()
        {
            foreach (IDisposable binding in base.GetButtonsBindings())
            {
                yield return binding;
            }
            yield return m_InstallButton.OnClickAsObservable().Subscribe(_ => ViewModel.Install());
        }

        protected override string GetNotCompletedString()
        {
            return "Без приложения никак!";
        }
    }
}