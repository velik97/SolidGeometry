using System;
using Serialization.LessonsFileSystem;
using UI.MVVM;
using UniRx;

namespace UI.MainMenu.Header
{
    public class MainMenuHeaderVM : ViewModel
    {
        private readonly ReactiveProperty<string> m_HeaderName = new ReactiveProperty<string>();
        private readonly ReactiveProperty<bool> m_CanGoBack = new ReactiveProperty<bool>();

        public IReadOnlyReactiveProperty<string> HeaderName => m_HeaderName;
        public IReadOnlyReactiveProperty<bool> CanGoBack => m_CanGoBack;

        private Action m_GoBackAction;

        public MainMenuHeaderVM(Action goBackAction)
        {
            m_GoBackAction = goBackAction;
        }

        public void SetFolder(FolderAsset folderAsset)
        {
            m_HeaderName.Value = folderAsset.AssetName;
            m_CanGoBack.Value = folderAsset.ParentFolder != null;
        }

        public void GoBack()
        {
            m_GoBackAction?.Invoke();
        }
    }
}