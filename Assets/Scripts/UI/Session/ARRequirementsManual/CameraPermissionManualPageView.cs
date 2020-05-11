using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Session.ARRequirementsManual
{
    public class CameraPermissionManualPageView : ManualPageView<CameraPermissionManualPageVM>
    {
        [SerializeField]
        private Image m_IOSPermissionSettingsManualImage;
        [SerializeField]
        private Image m_AndroidPermissionSettingsManualImage;

        [SerializeField]
        private Button m_GoToSettingsButton;

        public override void Bind(CameraPermissionManualPageVM viewModel)
        {
            base.Bind(viewModel);
            Add(m_GoToSettingsButton.OnClickAsObservable().Subscribe(_ => ViewModel.GoToPermissionSettings()));

            if (ViewModel.RunningPlatform == RuntimePlatform.Android)
            {
                m_IOSPermissionSettingsManualImage.gameObject.SetActive(false);
                m_AndroidPermissionSettingsManualImage.gameObject.SetActive(true);
            }
            else if (ViewModel.RunningPlatform == RuntimePlatform.IPhonePlayer)
            {
                m_IOSPermissionSettingsManualImage.gameObject.SetActive(true);
                m_AndroidPermissionSettingsManualImage.gameObject.SetActive(false);
            }
        }

        protected override string GetNotCompletedString()
        {
            return "Без камеры никак!";
        }
    }
}