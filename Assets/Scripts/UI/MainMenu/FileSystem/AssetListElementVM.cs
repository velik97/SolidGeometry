using System;
using Serialization.LessonsFileSystem;
using UI.MVVM;

namespace UI.MainMenu.FileSystem
{
    public class AssetListElementVM : ViewModel
    {
        private readonly FileSystemAsset m_FileSystemAsset;
        
        public string LessonName => m_FileSystemAsset.AssetName;
        public string Description => m_FileSystemAsset.Description;
        
        private readonly Action<FileSystemAsset> m_OnAssetChosen;

        public readonly bool IsFolder;

        public AssetListElementVM(FileSystemAsset fileSystemAsset, Action<FileSystemAsset> onAssetChosen)
        {
            m_FileSystemAsset = fileSystemAsset;
            m_OnAssetChosen = onAssetChosen;

            IsFolder = m_FileSystemAsset is FolderAsset;
        }

        public void ButtonPressed()
        {
            m_OnAssetChosen.Invoke(m_FileSystemAsset);
        }
    }
}