using TMPro;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lesson.TopPanel
{
    public class TopPanelView : View<TopPanelVM>
    {
        [SerializeField]
        private Button m_BackButton;

        [SerializeField]
        private GameObject m_ChangeButtonGameObject;
        [SerializeField]
        private Button m_ChangeModeButton;

        [SerializeField]
        private TextMeshProUGUI m_ChangeModeButtonLabel;

        public override void Bind(TopPanelVM viewModel)
        {
            base.Bind(viewModel);

            AddDisposable(m_BackButton.OnClickAsObservable().Subscribe(_ => ViewModel.OnBackPressed()));
            AddDisposable(m_ChangeModeButton.OnClickAsObservable().Subscribe(_ => ViewModel.OnChangeModePressed()));
            
            AddDisposable(ViewModel.ChangeModeButtonName.Subscribe(SetFunctionalButtonName));
            AddDisposable(ViewModel.CanChangeMode.Subscribe(m_ChangeButtonGameObject.SetActive));
        }

        private void SetFunctionalButtonName(string buttonName)
        {
            m_ChangeModeButtonLabel.text = buttonName;
        }
    }
}