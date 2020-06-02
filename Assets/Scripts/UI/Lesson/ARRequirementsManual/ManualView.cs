using System.Collections.Generic;
using DG.Tweening;
using Runtime.Access.UI;
using UI.MVVM;
using UniRx;
using UnityEngine;

namespace UI.Lesson.ARRequirementsManual
{
    public class ManualView : View<ManualVM>
    {
        [SerializeField]
        private CanvasGroup m_CanvasGroup;
        
        [SerializeField]
        private CameraPermissionManualPageView m_CameraPermissionManualView;
        [SerializeField]
        private InstallARSoftwareManualPageView m_InstallARSoftwareManualView;

        private List<IManualPageView> m_ManualPageViews;

        public void Initialize()
        {
            gameObject.SetActive(false);
            m_CanvasGroup.alpha = 0f;
        }

        public override void Bind(ManualVM viewModel)
        {
            base.Bind(viewModel);

            m_ManualPageViews = new List<IManualPageView>();
            
            foreach (ManualPageVM pageVM in ViewModel.ManualPageVMs)
            {
                switch (pageVM)
                {
                    case CameraPermissionManualPageVM cameraPermissionManualPageVM:
                        m_CameraPermissionManualView.Bind(cameraPermissionManualPageVM);
                        m_ManualPageViews.Add(m_CameraPermissionManualView);
                        break;
                    case InstallARSoftwareManualPageVM installARSoftwareManualPageVM:
                        m_InstallARSoftwareManualView.Bind(installARSoftwareManualPageVM);
                        m_ManualPageViews.Add(m_InstallARSoftwareManualView);
                        break;
                }
            }
            
            foreach (IManualPageView pageView in m_ManualPageViews)
            {
                pageView.DisappearImmediate();
            }

            AddDisposable(ViewModel.CurrentPageNumber.Subscribe(SetCurrentPageIndex));
        }

        private void Appear()
        {
            gameObject.SetActive(true);
            m_CanvasGroup.alpha = 0f;
            m_ManualPageViews[0].AppearImmediate();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(m_CanvasGroup.DOFade(1f, UIConsts.InteractionTime));
            sequence.AppendCallback(m_ManualPageViews[0].BindButtons);
        }

        private void Disappear()
        {
            foreach (IManualPageView pageView in m_ManualPageViews)
            {
                pageView.UnbindButtons();
            }

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(UIConsts.InteractionTimeShort);
            sequence.Append(m_CanvasGroup.DOFade(0f, UIConsts.InteractionTime));
            sequence.AppendCallback(() => gameObject.SetActive(false));
        }

        private void SetCurrentPageIndex(int index)
        {
            if (index < 0 || index >= m_ManualPageViews.Count)
            {
                return;
            }

            if (index == 0)
            {
                Appear();
                return;
            }

            m_ManualPageViews[index - 1].Disappear();
            m_ManualPageViews[index].Appear();
        }

        protected override void UnbindViewAction()
        {
            Disappear();
        }
    }
}