﻿using System;
using Runtime;
using Runtime.Access.Lesson;
using Serialization.LessonsFileSystem;
using UI.MainMenu.FileSystem;
using UI.MainMenu.Header;
using UnityEngine;
using Util.UniRxExtensions;

namespace UI.MainMenu
{
    public class MainMenuUIRunner : MonoBehaviourMultipleDisposable, ISceneRunner
    {
        [SerializeField]
        private AssetListView m_AssetListView;
        private AssetListVM m_AssetListVM;

        [SerializeField]
        private MainMenuHeaderView m_HeaderView;
        private MainMenuHeaderVM m_HeaderVM;
        
        private FolderAsset m_CurrentFolder;

        public void Initialize()
        {
            InitializeLessonsList();
            InitializeHeader();
            
            SetInitialFolder();
        }

        private void InitializeLessonsList()
        {
            m_AssetListVM = new AssetListVM(OnAssetChosen);
            AddDisposable(m_AssetListVM);
            m_AssetListView.Bind(m_AssetListVM);
        }

        private void InitializeHeader()
        {
            m_HeaderVM = new MainMenuHeaderVM(GoToPreviousFolder);
            AddDisposable(m_HeaderVM);
            m_HeaderView.Bind(m_HeaderVM);
        }

        private void SetInitialFolder()
        {
            FolderAsset folderAsset = LessonAccess.Instance.RootFolder;
            if (LessonAccess.Instance.CurrentLessonAsset?.ParentFolder != null)
            {
                folderAsset = LessonAccess.Instance.CurrentLessonAsset.ParentFolder;
            }

            SetFolder(folderAsset);
        }

        private void SetFolder(FolderAsset folderAsset)
        {
            m_CurrentFolder = folderAsset;
            m_AssetListVM.SetFolder(folderAsset);
            m_HeaderVM.SetFolder(folderAsset);
        }

        private void OnAssetChosen(FileSystemAsset asset)
        {
            if (asset is FolderAsset folderAsset)
            {
                SetFolder(folderAsset);
            }
            else if (asset is LessonAsset lessonAsset)
            {
                LessonAccess.Instance.RequestStartLesson(lessonAsset);
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