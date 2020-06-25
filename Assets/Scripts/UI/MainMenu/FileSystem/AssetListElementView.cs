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
        [SerializeField]
        private Image m_GoNextButton;
        [SerializeField]
        private Image isBlockedButton;
        
        [SerializeField]
        private Button UnlockLibraryButton;

        private Image frame;

        private float fontSize11 = 24;
        private float fontSize12 = 16;
        private float fontSize21 = 16;
        private float fontSize22 = 24;

        public override void Bind(AssetListElementVM viewModel)
        {
            m_FolderImage.gameObject.SetActive(false);

            //   m_FolderImage.color = viewModel.Color;
            //   m_FolderImage.enabled = false;
            //   m_FolderImage.transform.GetChild(0).gameObject.SetActive(false);

            m_GoNextButton.gameObject.SetActive(viewModel.IsFolder);
            m_GoNextButton.color = viewModel.Color;

            isBlockedButton.gameObject.SetActive(!viewModel.IsFolder && viewModel.isBlocked);
            isBlockedButton.color = viewModel.Color;
            // Set color of the frame
            frame = GetComponent<Image>();
            frame.color = viewModel.Color;
            
            base.Bind(viewModel);

            m_LessonNameLabel.text = ViewModel.LessonName;
            m_LessonNameLabel.color = viewModel.Color;

            if (ViewModel.IsFolder)
            {
                m_LessonNameLabel.fontSize = fontSize11;
                m_DescriptionLabel.fontSize = fontSize12;
            }

            if (!ViewModel.IsFolder)
            {
                m_LessonNameLabel.fontSize = fontSize21;
                m_DescriptionLabel.fontSize = fontSize22;
                
                if (ViewModel.isUnlockLibraryButton)
                {
                    m_FolderImage.gameObject.SetActive(false);
                 //   m_ChoseLessonButton.gameObject.SetActive(false);
                    m_LessonNameLabel.gameObject.SetActive(false);
                    m_DescriptionLabel.gameObject.SetActive(false);
                    m_FolderImage.gameObject.SetActive(false);
                    m_GoNextButton.gameObject.SetActive(false);
                    isBlockedButton.gameObject.SetActive(false);
                  
                    UnlockLibraryButton.gameObject.SetActive(true);
                    UnlockLibraryButton.image.color = viewModel.Color;
                }
            }
            
           // UnlockLibraryButton.gameObject.SetActive(!viewModel.IsFolder && viewModel.isUnlockLibraryButton);
           // UnlockLibraryButton.image.color = viewModel.Color;
            

            m_DescriptionLabel.text = ViewModel.Description;
            
            AddDisposable(m_ChoseLessonButton.OnClickAsObservable().Subscribe(_ => ViewModel.ButtonPressed()));
        }

  //      public void Update()
  //      {
  //          if (!ViewModel.IsFolder)
  //              m_LessonNameLabel.fontSize = fontSize1;
  //      }
    }
}