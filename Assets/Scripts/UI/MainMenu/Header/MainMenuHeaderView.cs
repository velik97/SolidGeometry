using TMPro;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.Header
{
    public class MainMenuHeaderView : View<MainMenuHeaderVM>
    {
        [SerializeField]
        private TextMeshProUGUI m_HeaderLabel;

        [SerializeField]
        private Button m_Button;

        public override void Bind(MainMenuHeaderVM viewModel)
        {
            base.Bind(viewModel);
            
            Add(ViewModel.HeaderName.Subscribe(text => m_HeaderLabel.text = text));
            Add(ViewModel.CanGoBack.Subscribe(value => m_Button.gameObject.SetActive(value)));

            Add(m_Button.OnClickAsObservable().Subscribe(_ => ViewModel.GoBack()));
        }
    }
}