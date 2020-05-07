using System;
using Runtime;
using Runtime.Core;
using Serialization.LessonsFileSystem;
using UI.MainMenu.FileSystem;
using UI.MainMenu.Header;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.MainMenu
{
    public class MainMenuUIRunner : MonoBehaviourCompositeDisposable, ISceneRunner
    {
        [SerializeField]
        private AssetListView m_AssetListView;
        private AssetListVM m_AssetListVM;

        [SerializeField]
        private MainMenuHeaderView m_HeaderView;
        private MainMenuHeaderVM m_HeaderVM;
        
        private GlobalData m_GlobalData;
        private FolderAsset m_CurrentFolder;

        public void Initialize(GlobalData globalData)
        {
            m_GlobalData = globalData;
            
            InitializeLessonsList();
            InitializeHeader();
            
            SetInitialFolder();
        }

        private void InitializeLessonsList()
        {
            m_AssetListVM = new AssetListVM(OnLessonChosen);
            Add(m_AssetListVM);
            m_AssetListView.Bind(m_AssetListVM);
        }

        private void InitializeHeader()
        {
            m_HeaderVM = new MainMenuHeaderVM(GoToPreviousFolder);
            Add(m_HeaderVM);
            m_HeaderView.Bind(m_HeaderVM);
        }

        private void SetInitialFolder()
        {
            FolderAsset folderAsset = m_GlobalData.RootFolder;
            if (m_GlobalData.CurrentLessonAsset?.ParentFolder != null)
            {
                folderAsset = m_GlobalData.CurrentLessonAsset.ParentFolder;
            }

            SetFolder(folderAsset);
        }

        private void SetFolder(FolderAsset folderAsset)
        {
            m_CurrentFolder = folderAsset;
            m_AssetListVM.SetFolder(folderAsset);
            m_HeaderVM.SetFolder(folderAsset);
        }

        private void OnLessonChosen(FileSystemAsset asset)
        {
            if (asset is FolderAsset folderAsset)
            {
                SetFolder(folderAsset);
            }
            else if (asset is LessonAsset lessonAsset)
            {
                m_GlobalData.StartLesson(lessonAsset);
            }
        }

        private void GoToPreviousFolder()
        {
            if (m_CurrentFolder?.ParentFolder == null)
            {
                return;
            }

            SetFolder(m_CurrentFolder.ParentFolder);
        }

        public void Unload(Action callback)
        {
            Dispose();
            callback.Invoke();
        }
    }
}