using TMPro;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Session.CloseLesson
{
    public class TopPanelView : View<TopPanelVM>
    {
        [SerializeField]
        private Button m_BackButton;

        [SerializeField]
        private Button m_FunctionalButton;

        [SerializeField]
        private TextMeshProUGUI m_FunctionalButtonLabel;

        public override void Bind(TopPanelVM viewModel)
        {
            base.Bind(viewModel);

            Add(m_BackButton.OnClickAsObservable().Subscribe(_ => ViewModel.OnBackPressed()));
            Add(m_FunctionalButton.OnClickAsObservable().Subscribe(_ => ViewModel.OnFunctionalPressed()));
            
            Add(ViewModel.FunctionalButtonName.Subscribe(SetFunctionalButtonName));
        }

        private void SetFunctionalButtonName(string buttonName)
        {
            m_FunctionalButtonLabel.text = buttonName;
        }
    }
}