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
        private GameObject m_ChangeButtonGameObject;
        [SerializeField]
        private Button m_ChangeModeButton;

        [SerializeField]
        private TextMeshProUGUI m_ChangeModeButtonLabel;

        public override void Bind(TopPanelVM viewModel)
        {
            base.Bind(viewModel);

            Add(m_BackButton.OnClickAsObservable().Subscribe(_ => ViewModel.OnBackPressed()));
            Add(m_ChangeModeButton.OnClickAsObservable().Subscribe(_ => ViewModel.OnChangeModePressed()));
            
            Add(ViewModel.ChangeModeButtonName.Subscribe(SetFunctionalButtonName));
            Add(ViewModel.CanChangeMode.Subscribe(m_ChangeButtonGameObject.SetActive));
        }

        private void SetFunctionalButtonName(string buttonName)
        {
            m_ChangeModeButtonLabel.text = buttonName;
        }
    }
}