using TMPro;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.FileSystem
{
    public class AssetListElementView : View<AssetListElementVM>
    {
        [SerializeField]
        private Button m_ChoseLessonButton;

        [SerializeField]
        private TextMeshProUGUI m_LessonNameLabel;
        [SerializeField]
        private TextMeshProUGUI m_DescriptionLabel;

        [SerializeField]
        private Image m_FolderImage;

        public override void Bind(AssetListElementVM viewModel)
        {
            base.Bind(viewModel);

            m_LessonNameLabel.text = ViewModel.LessonName;
            m_DescriptionLabel.text = ViewModel.Description;

            m_FolderImage.gameObject.SetActive(ViewModel.IsFolder);
            
            AddDisposable(m_ChoseLessonButton.OnClickAsObservable().Subscribe(_ => ViewModel.ButtonPressed()));
        }
    }
}