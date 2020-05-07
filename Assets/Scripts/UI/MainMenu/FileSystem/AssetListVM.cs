using System;
using System.Collections.Generic;
using Serialization.LessonsFileSystem;
using UI.MVVM;
using UniRx;

namespace UI.MainMenu.FileSystem
{
    public class AssetListVM : ViewModel
    {
        private readonly List<AssetListElementVM> m_ElementVMs;
        public IReadOnlyList<AssetListElementVM> ElementVMs => m_ElementVMs;
        
        public readonly ReactiveCommand OnListUpdated = new ReactiveCommand();

        private readonly Action<FileSystemAsset> m_OnAssetChosen;

        public AssetListVM(Action<FileSystemAsset> onAssetChosen)
        {
            m_ElementVMs = new List<AssetListElementVM>();
            m_OnAssetChosen = onAssetChosen;
        }

        public void SetFolder(FolderAsset folderAsset)
        {
            m_ElementVMs.Clear();
            foreach (FileSystemAsset asset in folderAsset.AssetsList)
            {
                AssetListElementVM elementVM = new AssetListElementVM(asset, m_OnAssetChosen);
                Add(elementVM);
                m_ElementVMs.Add(elementVM);
            }

            OnListUpdated.Execute();
        }
    }
}